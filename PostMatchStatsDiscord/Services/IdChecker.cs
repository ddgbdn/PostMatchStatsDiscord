using System.Text.Json;
using System.Text.Json.Serialization;
using PostMatchStatsDiscord.Models;
using PostMatchStatsDiscord.Constants;
using PostMatchStatsDiscord.Services;

namespace PostMatchStatsDiscord.Services
{
    public class IdChecker
    {
        public async Task<bool> IsObtained(long id)
        {
            var obtainedIds = await JsonIO.ReadJsonAsync<IdHash>(Paths.ObtainedMathesPath);
            return obtainedIds.Ids.Contains(id);
        }
    }
}
