using System.Text.Json.Serialization;

namespace Entities.Models
{
    public class Stats
    {
        [JsonPropertyName("actionsPerMinute")]
        public long[] ActionsPerMinute { get; set; } = null!;
    }
}
