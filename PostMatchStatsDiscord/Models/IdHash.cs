using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PostMatchStatsDiscord.Models
{
    class IdHash
    {
        [JsonPropertyName("ids")]
        public List<long> Ids { get; set; }
    }
}
