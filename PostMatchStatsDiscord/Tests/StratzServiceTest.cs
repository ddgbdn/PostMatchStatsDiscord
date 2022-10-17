using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PostMatchStatsDiscord.Models;
using PostMatchStatsDiscord.Services;

namespace PostMatchStatsDiscord.Tests
{
   
    internal class StratzServiceTest
    {
        delegate Task<MatchStats> StratzTest(long id);
        [Test]
        public void DebugTest()
        {
            Assert.DoesNotThrowAsync(() => StratzService.GetLastMatchAsync(6808030803));
        }

        [Test]
        public void DebugId()
        {
            Assert.DoesNotThrowAsync(StratzService.GetLastMatchIdAsync);
            var ids = StratzService.GetLastMatchIdAsync();
            Assert.GreaterOrEqual(ids.Result.Count(), 1);
        }

        [Test]
        public void TestToken()
        {
            var vars = System.Environment.GetEnvironmentVariables();
            Assert.IsNotNull(System.Environment.GetEnvironmentVariable("StratzAuthToken"));
        }
    }
}
