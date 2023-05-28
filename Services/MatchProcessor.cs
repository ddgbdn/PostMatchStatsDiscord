using Contracts;
using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.WebSocket;
using Entities.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class MatchProcessor : DiscordClientService
    {
        private readonly IServiceProvider _serviceProvider;

        public MatchProcessor(
            DiscordSocketClient client,
            IServiceProvider serviceProvider,
            ILogger<DiscordClientService> logger
            ) : base(client, logger)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Client.WaitForReadyAsync(stoppingToken);

            using PeriodicTimer timer = new(TimeSpan.FromMinutes(3));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                IEnumerable<ChannelSubscribers> subs;
                using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    subs = await scope.ServiceProvider.GetRequiredService<ISubscriberDataService>().GetSubscribers();
                }

                var channelMatchesToProcess = await GetMatchesForChannels(subs);

                var matchesToPost = await GetMatchStatsForChannels(channelMatchesToProcess);

                await PostMatches(matchesToPost);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task PostMatches(IEnumerable<ChannelMatches> matchesToPost)
        {
            using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();

                await Task.WhenAll(matchesToPost
                        .Select(async c => await Task.WhenAll(c.Matches
                            .Select(async m => await messageService.SendMessage(m, c.ChannelId)))));

                var matchDataService = scope.ServiceProvider.GetRequiredService<IMatchDataService>();

                await matchDataService.AddMatches(matchesToPost.Select(m =>
                    new ChannelMatchesIds
                    {
                        ChannelId = m.ChannelId,
                        Matches = m.Matches.Select(s => s.Id)
                    }));
            }
        }


        private async Task<IEnumerable<ChannelMatches>> GetMatchStatsForChannels(IEnumerable<ChannelMatchesIds> channelMatchesToProcess)
        {
            using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var stratzClient = scope.ServiceProvider.GetRequiredService<IStratzClient>();

                return await Task.WhenAll(channelMatchesToProcess
                    .Select(async c => new ChannelMatches
                    {
                        ChannelId = c.ChannelId,
                        Matches = await Task.WhenAll(c.Matches
                            .Select(async m => await stratzClient.GetMatchByIdAsync(m)))
                    }));
            }
        }

        private async Task<IEnumerable<ChannelMatchesIds>> GetMatchesForChannels(IEnumerable<ChannelSubscribers> subs)
        {
            using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var previouslyParsedMatches = await scope.ServiceProvider.GetRequiredService<IMatchDataService>().GetMatches();                

                var matches = await scope.ServiceProvider.GetRequiredService<IStratzClient>().GetLastMatchesIdsAsync(subs);

                var result = new List<ChannelMatchesIds>();

                foreach (var match in matches)
                {
                    var channel = previouslyParsedMatches.Single(p => p.ChannelId == match.ChannelId);
                    var nonRepeatingMatches = match.Matches.Where(m => !channel.Matches.Contains(m));
                    result.Add(new ChannelMatchesIds { ChannelId = match.ChannelId, Matches = nonRepeatingMatches });
                }

                return result;
            }
        }
    }
}
