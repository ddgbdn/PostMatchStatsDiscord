namespace Entities.Models
{
    public class ChannelMatches
    {
        public ulong ChannelId { get; set; }
        public IEnumerable<MatchStats> Matches { get; set; } = Enumerable.Empty<MatchStats>();
    }
}
