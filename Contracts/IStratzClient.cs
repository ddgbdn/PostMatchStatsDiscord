using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IStratzClient
    {
        public Task<IEnumerable<ChannelMatchesIds>> GetLastMatchesIdsAsync(IEnumerable<ChannelSubscribers> subs);
        public Task<MatchStats> GetMatchByIdAsync(long id);
        public Task<bool> ValidatePLayer(long id);
    }
}
