using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IMatchDataService
    {
        public Task<IEnumerable<ChannelMatchesIds>> GetMatches();
        public Task AddMatches(IEnumerable<ChannelMatchesIds> matches);
    }
}
