using Entities.Models;

namespace Contracts
{
    public interface IMessageService
    {
        public Task SendMessage(MatchStats match, ulong channelId);
    }
}
