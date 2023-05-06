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
        public Task<IEnumerable<long>> GetLastMatchesIdsAsync();
        public Task<MatchStats> GetMatchByIdAsync(long id);
    }
}
