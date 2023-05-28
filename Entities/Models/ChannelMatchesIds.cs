namespace Entities.Models
{
    public class ChannelMatchesIds
    {
        public ulong ChannelId { get; set; }
        public IEnumerable<long> Matches { get; set; } = Enumerable.Empty<long>();
    }
}
