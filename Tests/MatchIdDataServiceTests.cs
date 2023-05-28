using Services;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class MatchIdDataServiceTests
    {
        [Test]
        public void AddMatch()
        {
            Assert.DoesNotThrowAsync(async () => await new MatchIdDataService().AddMatches(
                new ChannelMatchesIds[]
                {
                    new ChannelMatchesIds
                    {
                        ChannelId = 1104500906621415606,
                        Matches = new List<long>
                        {
                            7172221745,
                            7172102881
                        }
                    },
                    new ChannelMatchesIds
                    {
                        ChannelId = 1,
                        Matches = new List<long>
                        {
                            7172221745,
                            7172000587
                        }
                    }
                }));
        }
    }
}
