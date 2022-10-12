using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PostMatchStatsDiscord.Services;

namespace PostMatchStatsDiscord.Tests
{
   
    internal class StratzServiceTest
    {
        [Test]
        public void DebugTest()
        {
            Assert.DoesNotThrow(new StratzService().GetLastMatch);
        }

        [Test]
        public void TestToken()
        {
            var vars = System.Environment.GetEnvironmentVariables();
            Assert.IsNotNull(System.Environment.GetEnvironmentVariable("StratzAuthToken"));
        }


    }
}
