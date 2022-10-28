using Discord.WebSocket;
using Discord;
using NUnit.Framework;
using PostMatchStatsDiscord.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostMatchStatsDiscord.Tests
{
    [TestFixture]
    class MessageServiceTests
    {
        [Test]
        public async Task ButtPlugTest()
        {
            var _client = new DiscordSocketClient();

            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("StratzDiscordBotToken"));

            await _client.StartAsync();

            _client.Ready += () =>
            {
                Console.WriteLine("Bot is connected!");
                return Task.CompletedTask;
            };
            Assert.DoesNotThrowAsync(new MessageService(_client).SendMessage);
        }
    }
}
