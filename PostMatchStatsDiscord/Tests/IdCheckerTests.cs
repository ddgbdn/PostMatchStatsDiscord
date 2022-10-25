using NUnit.Framework;
using PostMatchStatsDiscord.Services;

namespace PostMatchStatsDiscord.Tests
{
    [TestFixture]
    internal class IdCheckerTests
    {
        [Test]
        public void DebugObtained()
        {
            Assert.DoesNotThrowAsync(() => new IdChecker().IsObtained(6808030803));
        }

        [Test]
        public void Checks()
        {
            Assert.IsTrue(new IdChecker().IsObtained(1).Result);
        }
    }
}
