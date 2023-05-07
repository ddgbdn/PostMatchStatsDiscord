using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ChannelSubscribers
    {
        public ulong ChannelId { get; set; }
        public IEnumerable<long> Subscribers { get; set; } = Enumerable.Empty<long>();
    }
}
