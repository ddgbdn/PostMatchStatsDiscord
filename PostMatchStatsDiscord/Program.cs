﻿using Discord;
using Discord.WebSocket;


public class Program
{
    public static Task Main(string[] args)
    {
        var a = 0;
        return new Program().MainAsync();
    }

    private DiscordSocketClient _client;

    public async Task MainAsync()
    { 
        _client = new DiscordSocketClient();

        _client.Log += Log;

        var token = Environment.GetEnvironmentVariable("StratzDiscordBotToken");

        await _client.LoginAsync(TokenType.Bot, token);

        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}