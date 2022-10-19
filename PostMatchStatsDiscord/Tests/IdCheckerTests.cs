using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostMatchStatsDiscord.Services;

namespace PostMatchStatsDiscord.Tests
{
    [TestFixture]
    internal class IdCheckerTests
    {
        [Test]
        public void DebugObtained()
        {
            Assert.DoesNotThrowAsync(() => IdChecker.IsObtained(6808030803));
        }

        [Test]
        public void DebugNotParsed()
        {
            Assert.DoesNotThrowAsync(() => IdChecker.IsObtained(6808030803));
        }
    }
}
