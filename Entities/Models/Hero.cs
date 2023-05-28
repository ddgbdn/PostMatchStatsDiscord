using System.Text.Json.Serialization;

namespace Entities.Models
{
    public class Hero
    {
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = null!;
    }
}
