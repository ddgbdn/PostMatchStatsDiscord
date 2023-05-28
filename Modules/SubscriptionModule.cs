using Contracts;
using Discord.Interactions;
using Entities.Models;
using Microsoft.Extensions.DependencyInjection;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules
{
    public class SubscriptionModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IServiceProvider _serviceProvider;

        public SubscriptionModule(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [SlashCommand("subscribe", "Subscribe to user to listen to")]
        public async Task SubscribeUser([Summary(description: "Ingame player's ID")] long id)
        {
            try
            {
                using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    await scope.ServiceProvider.GetRequiredService<ISubscriberDataService>().AddSubscriber(
                    new ChannelSubscribers
                    {
                        ChannelId = Context.Channel.Id,
                        Subscribers = new long[] { id }
                    });
                }
                await RespondAsync("Registered");
            }
            catch (ArgumentException ex)
            {
                await RespondAsync(ex.Message);
            }
        }
    }
}
