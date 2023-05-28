using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ISubscriberDataService
    {
        public Task<IEnumerable<ChannelSubscribers>> GetSubscribers();
        public Task AddSubscriber(ChannelSubscribers newSubscriber);
    }
}
