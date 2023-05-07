using Contracts;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
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
        //Add any other services here
        //services.AddHostedService<CommandHandler>();
        //services.AddHostedService<InteractionHandler>();
        //services.AddHostedService<BotStatusService>();
        //services.AddHostedService<LongRunningService>();
        services.AddHostedService<MatchProcessor>();
        services.AddSingleton<IStratzClient, StratzClient>();
        services.AddSingleton<IMessageService, MessageService>();
    }).Build();

await host.RunAsync();