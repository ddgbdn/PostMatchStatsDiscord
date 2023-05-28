using Entities.Models;

namespace Contracts
{
    public interface IMatchDataService
    {
        public Task<IEnumerable<ChannelMatchesIds>> GetMatches();
        public Task AddMatches(IEnumerable<ChannelMatchesIds> matches);
    }
}
