using Entities.Models;
using Services;

namespace Tests
{
    public class SubscriberDataServiceTests
    {
        [Test]
        public void AddsSubs()
        {
            Assert.DoesNotThrowAsync(async () => await new SubscriberDataService(new StratzClient()).AddSubscriber(
                new ChannelSubscribers
                {
                    ChannelId = 1104500906621415606,
                    Subscribers = new long[] { 236888271 }
                })); 
        }
    }
}