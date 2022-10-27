using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace PostMatchStatsDiscord.Services
{
    public class MessageService
    {
        private DiscordSocketClient _client;

        public MessageService(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task SendMessage()
        {
            var channel = await _client.GetChannelAsync(370233530346766338) as IMessageChannel;
            await channel.SendMessageAsync()
        }
    }
}
