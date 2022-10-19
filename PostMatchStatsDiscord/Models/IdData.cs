namespace PostMatchStatsDiscord.Models
{
    using System.Text.Json.Serialization;

    public class IdData
    {
        [JsonPropertyName("players")]
        public PlayerSteam[] Players { get; set; }
    }

    public class PlayerSteam
    {
        [JsonPropertyName("matches")]
        public Match[] Matches { get; set; }
    }

    public class Match
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}
