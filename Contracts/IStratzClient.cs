using Entities.Models;

namespace Contracts
{
    public interface IStratzClient
    {
        public Task<IEnumerable<ChannelMatchesIds>> GetLastMatchesIdsAsync(IEnumerable<ChannelSubscribers> subs);
        public Task<MatchStats> GetMatchByIdAsync(long id);
        public Task<bool> ValidatePLayer(long id);
    }
}
