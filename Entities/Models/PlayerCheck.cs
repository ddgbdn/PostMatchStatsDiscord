using System.Text.Json.Serialization;

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
