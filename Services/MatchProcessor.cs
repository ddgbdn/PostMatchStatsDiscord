using Contracts;
using Discord.Addons.Hosting;
using Discord.Addons.Hosting.Util;
using Discord.Commands;
using Discord.WebSocket;
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

            using PeriodicTimer timer = new(TimeSpan.FromSeconds(1));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                var matchesToProcess = await _stratzClient.GetLastMatchesIdsAsync();
                var matchesToPost = await Task.WhenAll(
                    matchesToProcess.Select(async m => await _stratzClient.GetMatchByIdAsync(m)));

                matchesToPost.Select(m => _messageService.SendMessage(m, 1104500906621415606)); //To enter id corresponding to guild
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                Logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
