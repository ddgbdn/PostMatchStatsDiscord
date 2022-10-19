using NUnit.Framework;
using PostMatchStatsDiscord.Models;
using PostMatchStatsDiscord.Services;

namespace PostMatchStatsDiscord.Tests
{
    [TestFixture]
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
            var vars = Environment.GetEnvironmentVariables();
            Assert.IsNotNull(Environment.GetEnvironmentVariable("StratzAuthToken"));
        }
    }
}
