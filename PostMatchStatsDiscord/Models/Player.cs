using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace PostMatchStatsDiscord.Models
{
    enum MatchPlayerAward
    {
        MVP,
        TOP_CORE,
        TOP_SUPPORT,
        NONE
    }

    internal class Player
    {
        public SteamAccount SteamAccount { get; set; }
        public Hero Hero { get; set; }

        [JsonPropertyName("imp")]
        public int Impact { get; set; } 
        public MatchPlayerAward Award { get; set; }
    }

    class SteamAccount
    {
        public string Name { get; set; }
        public long Id { get; set; }
    }

    class Hero
    {
        public string Name { get; set; }
    }   
}
