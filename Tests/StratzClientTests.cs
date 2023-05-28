using Entities.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class StratzClientTests
    {
        [Test]
        public void CheckId()
        {
            Assert.DoesNotThrowAsync(async () => await new StratzClient().ValidatePLayer(236888270));
        }
    }
}
