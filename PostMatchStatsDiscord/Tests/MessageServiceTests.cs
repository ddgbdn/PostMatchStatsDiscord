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
        DiscordSocketClient _client;
        MessageService _service;

        [SetUp]
        public async Task Init()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("StratzDiscordBotToken"));

            await _client.StartAsync();

            _client.Ready += () =>
            {
                Console.WriteLine("Bot is connected!");
                return Task.CompletedTask;
            };

            _service = new MessageService(_client);
        }

        [Test]
        public async Task ButtPlugTest()
        {               
            //Assert.DoesNotThrowAsync(_service.SendMessage);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
