using System.Text.Json.Serialization;

namespace Entities.Models
{
    public class SteamAccount
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
    }
}
