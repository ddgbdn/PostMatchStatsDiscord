using Discord;
using Discord.WebSocket;
using PostMatchStatsDiscord.Services;

public class Program
{
    public static Task Main(string[] args)
        => new Program().MainAsync();

    private DiscordSocketClient _client;

    public async Task MainAsync()
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
        await new MessageService(_client).SendMessage();

        await new Processor().StartAsync();

        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}