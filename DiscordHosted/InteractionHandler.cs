using Discord.Addons.Hosting;
using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord.Addons.Hosting.Util;

namespace DiscordHosted
{
    public class InteractionHandler : DiscordClientService
    {
        private readonly IServiceProvider _provider;
        private readonly InteractionService _interactionService;
        private readonly IHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public InteractionHandler(
            DiscordSocketClient client,
            ILogger<DiscordClientService> logger,
            IServiceProvider provider,
            InteractionService interactionService,
            IHostEnvironment environment,
            IConfiguration configuration) : base(client, logger)
        {
            _provider = provider;
            _interactionService = interactionService;
            _environment = environment;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Process the InteractionCreated payloads to execute Interactions commands
            Client.InteractionCreated += HandleInteraction;

            // Process the command execution results 
            _interactionService.SlashCommandExecuted += SlashCommandExecuted;

            await _interactionService.AddModulesAsync(typeof(Modules.AssemblyReference).Assembly, _provider);
            await Client.WaitForReadyAsync(stoppingToken);

            // If DOTNET_ENVIRONMENT is set to development, only register the commands to a single guild
            if (_environment.IsDevelopment())
                await _interactionService.RegisterCommandsToGuildAsync(_configuration.GetValue<ulong>("DevGuild"));
            else
                await _interactionService.RegisterCommandsGloballyAsync();
        }

        private Task SlashCommandExecuted(SlashCommandInfo commandInfo, IInteractionContext context, IResult result)
            => Task.CompletedTask;
        
        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {
                // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules
                var ctx = new SocketInteractionContext(Client, arg);
                await _interactionService.ExecuteCommandAsync(ctx, _provider);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Exception occurred whilst attempting to handle interaction.");

                if (arg.Type == InteractionType.ApplicationCommand)
                {
                    var msg = await arg.GetOriginalResponseAsync();
                    await msg.DeleteAsync();
                }

            }
        }
    }
}
