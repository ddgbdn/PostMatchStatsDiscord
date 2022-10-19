namespace PostMatchStatsDiscord.Models
{
    using System.Text.Json.Serialization;

    public class MatchData
    {
        [JsonPropertyName("match")]
        public MatchStats Match { get; set; }
    }

    public class MatchStats
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("parsedDateTime")]
        public long ParsedDateTime { get; set; }

        [JsonPropertyName("didRadiantWin")]
        public bool DidRadiantWin { get; set; }

        [JsonPropertyName("durationSeconds")]
        public long DurationSeconds { get; set; }

        [JsonPropertyName("averageRank")]
        public object AverageRank { get; set; }

        [JsonPropertyName("players")]
        public Player[] Players { get; set; }
    }

    public class Player
    {
        [JsonPropertyName("steamAccount")]
        public SteamAccount SteamAccount { get; set; }

        [JsonPropertyName("isRadiant")]
        public bool IsRadiant { get; set; }

        [JsonPropertyName("hero")]
        public Hero Hero { get; set; }

        [JsonPropertyName("position")]
        public string Position { get; set; }

        [JsonPropertyName("imp")]
        public long Imp { get; set; }

        [JsonPropertyName("award")]
        public string Award { get; set; }

        [JsonPropertyName("stats")]
        public Stats Stats { get; set; }
    }

    public class Hero
    {
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
    }

    public class Stats
    {
        [JsonPropertyName("actionsPerMinute")]
        public long[] ActionsPerMinute { get; set; }
    }

    public class SteamAccount
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
