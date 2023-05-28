using Contracts;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using DiscordHosted;
using Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureDiscordHost((context, config) =>
    {
        config.SocketConfig = new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose,
            AlwaysDownloadUsers = true,
            MessageCacheSize = 200
        };
        config.Token = Environment.GetEnvironmentVariable("StratzDiscordBotToken")!;
    })
    .UseInteractionService((context, config) =>
    {
        config.LogLevel = LogSeverity.Info;
        config.UseCompiledLambda = true;
    })
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<MatchProcessor>();
        services.AddScoped<IStratzClient, StratzClient>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IMatchDataService, MatchIdDataService>();
        services.AddScoped<ISubscriberDataService, SubscriberDataService>();
        services.AddHostedService<InteractionHandler>();
    }).Build();

await host.RunAsync();