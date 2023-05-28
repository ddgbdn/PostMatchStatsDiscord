namespace Entities.Models
{
    public class ChannelSubscribers
    {
        public ulong ChannelId { get; set; }
        public IEnumerable<long> Subscribers { get; set; } = Enumerable.Empty<long>();
    }
}
