using PostMatchStatsDiscord.Constants;
using PostMatchStatsDiscord.Models;
using System.Text.Json;

namespace PostMatchStatsDiscord.Services
{
    public static class JsonIO
    {
        public static async Task<T> ReadJsonAsync<T>(string path)
        {
            using FileStream stream = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }

        public static async Task AddIdsAsync(string path, IEnumerable<long> ids)
        {
            var json = await ReadJsonAsync<IdHash>(path);
            json.Ids.AddRange(ids);
            await File.WriteAllTextAsync(path, JsonSerializer.Serialize(json));
        }

        public static async Task DeleteNotParsedIdsAsync(IEnumerable<long> ids)
        {
            var json = await ReadJsonAsync<IdHash>(Paths.NotParsedMatchesPath);
            json.Ids.RemoveAll(id => ids.Contains(id));
            await File.WriteAllTextAsync(Paths.NotParsedMatchesPath, JsonSerializer.Serialize(json));
        }
    }
}
