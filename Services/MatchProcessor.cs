using Contracts;
using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.Commands;
using Discord.WebSocket;
using Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MatchProcessor : DiscordClientService
    {
        private readonly IServiceProvider _provider;
        private readonly IStratzClient _stratzClient;
        private readonly IMessageService _messageService;

        public MatchProcessor(
            DiscordSocketClient client,
            IServiceProvider provider,
            ILogger<DiscordClientService> logger,
            IStratzClient stratzClient,
            IMessageService messageService) : base(client, logger)
        {
            _provider = provider;
            _stratzClient = stratzClient;
            _messageService = messageService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Client.WaitForReadyAsync(stoppingToken);

            using PeriodicTimer timer = new(TimeSpan.FromMinutes(1));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                var channelMatchesToProcess = await _stratzClient.GetLastMatchesIdsAsync(
                    new ChannelSubscribers[] 
                    {
                        new ChannelSubscribers 
                        {
                            ChannelId = 1104500906621415606, 
                            Subscribers = new long[] { 236888270 } 
                        } 
                    }); // Fill with actual data

                var matchesToPost = await Task.WhenAll(
                    channelMatchesToProcess
                        .Select(async c => new ChannelMatches
                        {
                            ChannelId = c.ChannelId,
                            Matches = await Task.WhenAll(c.Matches
                                .Select(async m => await _stratzClient.GetMatchByIdAsync(m)))
                        }));

                await Task.WhenAll(matchesToPost
                    .Select(async c => await Task.WhenAll(c.Matches
                        .Select(async m => await _messageService.SendMessage(m, c.ChannelId)))));
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
