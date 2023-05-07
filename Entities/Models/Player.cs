using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Player
    {
        [JsonPropertyName("steamAccount")]
        public SteamAccount SteamAccount { get; set; } = null!;

        [JsonPropertyName("isRadiant")]
        public bool IsRadiant { get; set; }

        [JsonPropertyName("hero")]
        public Hero Hero { get; set; }

        [JsonPropertyName("kills")]
        public int Kills { get; set; }

        [JsonPropertyName("deaths")]
        public int Deaths { get; set; }

        [JsonPropertyName("assists")]
        public int Assists { get; set; }

        [JsonPropertyName("networth")]
        public int Networth { get; set; }

        [JsonPropertyName("heroDamage")]
        public int HeroDamage { get; set; }

        [JsonPropertyName("position")]
        public string Position { get; set; } = null!;

        [JsonPropertyName("imp")]
        public long Imp { get; set; }

        [JsonPropertyName("award")]
        public string Award { get; set; } = null!;

        [JsonPropertyName("stats")]
        public Stats Stats { get; set; } = null!;
    }
}
