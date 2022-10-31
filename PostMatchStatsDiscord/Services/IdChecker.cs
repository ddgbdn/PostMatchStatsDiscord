using PostMatchStatsDiscord.Constants;
using PostMatchStatsDiscord.Models;

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
