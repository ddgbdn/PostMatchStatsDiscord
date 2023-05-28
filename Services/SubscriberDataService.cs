using Contracts;
using Entities.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Services
{
    public class SubscriberDataService : ISubscriberDataService
    {
        private readonly IServiceProvider _serviceProvider;

        public SubscriberDataService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IEnumerable<ChannelSubscribers>> GetSubscribers()
        {
            using var stream = File.OpenRead(".\\Data\\Subscribers.txt");

            return await JsonSerializer.DeserializeAsync<ChannelSubscribers[]>(stream) ?? new ChannelSubscribers[] { };
        }

        public async Task AddSubscriber(ChannelSubscribers newSubscriber)
        {
            using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var client = scope.ServiceProvider.GetRequiredService<IStratzClient>();
                var matchService = scope.ServiceProvider.GetRequiredService<IMatchDataService>();

                var allSubs = (await GetSubscribers()).ToList();

                var existingChannel = allSubs.FirstOrDefault(s => s.ChannelId == newSubscriber.ChannelId);

                if (!await client.ValidatePLayer(newSubscriber.Subscribers.First()))
                    throw new ArgumentException("Player profile is anonymous or is not active");

                if (existingChannel != null)
                {
                    if (existingChannel.Subscribers.Contains(newSubscriber.Subscribers.First()))
                        throw new ArgumentException("User is already registered");
                    existingChannel.Subscribers = existingChannel.Subscribers.Concat(newSubscriber.Subscribers);
                }
                else
                    allSubs.Add(newSubscriber);

                await matchService.AddMatches(new ChannelMatchesIds[] 
                {
                    new ChannelMatchesIds 
                    { 
                        ChannelId = newSubscriber.ChannelId,
                        Matches = Enumerable.Empty<long>()
                    } 
                });

                var subsUtf8 = JsonSerializer.SerializeToUtf8Bytes(allSubs);

                await File.WriteAllBytesAsync(".\\Data\\Subscribers.txt", subsUtf8);
            }
        }
    }
}
