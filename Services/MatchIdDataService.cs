using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class MatchIdDataService : IMatchDataService
    {
        public async Task<IEnumerable<ChannelMatchesIds>> GetMatches()
        {
            using var stream = File.OpenRead(".\\Data\\ChannelMatches.txt");

            return await JsonSerializer.DeserializeAsync<ChannelMatchesIds[]>(stream) ?? new ChannelMatchesIds[] { };
        }

        public async Task AddMatches(IEnumerable<ChannelMatchesIds> newMatches)
        {
            var allMatches = (await GetMatches()).ToList();

            foreach (var match in newMatches)
            {
                var existingChannel = allMatches.FirstOrDefault(s => s.ChannelId == match.ChannelId);

                if (existingChannel != null)                
                    existingChannel.Matches = existingChannel.Matches.Concat(match.Matches);                
                else
                    allMatches.Add(match);
            }

            var subsUtf8 = JsonSerializer.SerializeToUtf8Bytes(allMatches);

            await File.WriteAllBytesAsync(".\\Data\\ChannelMatches.txt", subsUtf8);
        }
    }
}
