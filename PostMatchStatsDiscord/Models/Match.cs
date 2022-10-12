using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace PostMatchStatsDiscord.Models
{
    internal class Match
    {
        public long Id { get; set; }
        public bool DidRadiantWin { get; set; }
        public Player[] Players { get; set; }

        public override string ToString() 
            => JsonSerializer.Serialize(this, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
