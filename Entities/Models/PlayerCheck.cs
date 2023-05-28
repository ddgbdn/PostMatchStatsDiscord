using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class PlayerCheckData
    {
        [JsonPropertyName("player")]
        public PlayerCheck Player { get; set; } = null!;
    }

    public class PlayerCheck
    {
        public SteamAccountCheck steamAccount { get; set; } = null!;
        public long? lastMatchDate { get; set; }
    }

    public class SteamAccountCheck
    {
        public bool isAnonymous { get; set; }
    }
}
