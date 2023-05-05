using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Hero
    {
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = null!;
    }
}
