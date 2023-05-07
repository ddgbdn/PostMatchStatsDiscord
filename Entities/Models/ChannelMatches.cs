using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ChannelMatches
    {
        public ulong ChannelId { get; set; }
        public IEnumerable<MatchStats> Matches { get; set; } = Enumerable.Empty<MatchStats>();
    }
}
