using Entities.Models;

namespace Contracts
{
    public interface ISubscriberDataService
    {
        public Task<IEnumerable<ChannelSubscribers>> GetSubscribers();
        public Task AddSubscriber(ChannelSubscribers newSubscriber);
    }
}
