using NUnit.Framework;
using PostMatchStatsDiscord.Models;
using PostMatchStatsDiscord.Services;

namespace PostMatchStatsDiscord.Tests
{
    [TestFixture]
    internal class StratzServiceTest
    {
        [Test]
        public void DebugTest()
        {
            Assert.DoesNotThrowAsync(() => new StratzService().GetMatchByIdAsync(6808030803));
        }

        [Test]
        public void DebugId()
        {
            Assert.DoesNotThrowAsync(new StratzService().GetLastMatchIdAsync);
            var ids = new StratzService().GetLastMatchIdAsync();
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
