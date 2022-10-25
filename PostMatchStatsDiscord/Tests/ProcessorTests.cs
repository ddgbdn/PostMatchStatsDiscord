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
    internal class ProcessorTests
    {
        [Test]
        public void DebugStartAsync()
        {
            Assert.DoesNotThrowAsync(new Processor().StartAsync);
        }
    }
}
