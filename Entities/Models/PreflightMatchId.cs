using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class PreflightMatchId
    {
        [JsonPropertyName("players")]
        public PlayerSteam[] Players { get; set; } = null!;
    }

    public class PlayerSteam
    {
        [JsonPropertyName("steamAccountId")]
        public long SteamAccountId { get; set; }

        [JsonPropertyName("matches")]
        public Match[] Matches { get; set; } = null!;
    }

    public class Match
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("statsDateTime")]
        public long? ParsedDateTime { get; set; }
    }
}
