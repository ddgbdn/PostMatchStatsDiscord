using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ChannelMatchesIds
    {
        public ulong ChannelId { get; set; }
        public IEnumerable<long> Matches { get; set; } = Enumerable.Empty<long>();
    }
}
