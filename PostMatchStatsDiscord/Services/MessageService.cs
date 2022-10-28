using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using PostMatchStatsDiscord.Constants;

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
            using Stream fs = File.OpenRead(Paths.ButtPlug);
            await channel.SendFileAsync(fs, "buttplug");
        }
    }
}
