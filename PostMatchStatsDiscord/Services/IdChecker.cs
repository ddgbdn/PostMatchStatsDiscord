using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;

namespace PostMatchStatsDiscord.Services
{
    static class IdChecker
    {
        private static string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\");

        public static async Task<bool> IsObtained(long id)
        {
            var obtainedIds = await ReadJsonAsync<IdHash>(Path.GetFullPath(_path + "ObtainedMatches.json"));
            return obtainedIds.Ids.Contains(id);
        }
        
        private static async Task<T> ReadJsonAsync<T>(string path)
        {
            using FileStream stream = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }

        private class IdHash
        {
            [JsonPropertyName("ids")]
            public long[] Ids { get; set; }
        }
    }
}
