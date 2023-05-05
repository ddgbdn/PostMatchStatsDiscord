using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class MatchData
    {
        [JsonPropertyName("match")]
        public MatchStats Match { get; set; } = null!;
    }

    public class MatchStats
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("statsDateTime")]
        public long? ParsedDateTime { get; set; }

        [JsonPropertyName("didRadiantWin")]
        public bool DidRadiantWin { get; set; }

        [JsonPropertyName("durationSeconds")]
        public long DurationSeconds { get; set; }

        [JsonPropertyName("radiantKills")]
        public int[] RadiantKills { get; set; } = null!;

        [JsonPropertyName("direKills")]
        public int[] DireKills { get; set; } = null!;

        [JsonPropertyName("topLaneOutcome")]
        public string TopLaneOutcome { get; set; } = null!;

        [JsonPropertyName("midLaneOutcome")]
        public string MidLaneOutcome { get; set; } = null!;

        [JsonPropertyName("bottomLaneOutcome")]
        public string BottomLaneOutcome { get; set; } = null!;

        [JsonPropertyName("players")]
        public Player[] Players { get; set; } = null!;
    }
}
