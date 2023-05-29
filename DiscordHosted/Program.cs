using Contracts;
using Discord;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using DiscordHosted;
using Serilog;
using Serilog.Events;
using Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .WriteTo.Console()
    .WriteTo.File(".\\Logs\\log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting host");

    var host = Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureDiscordHost((context, config) =>
        {
            config.SocketConfig = new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                AlwaysDownloadUsers = true,
                MessageCacheSize = 200
            };
            config.Token = Environment.GetEnvironmentVariable("StratzDiscordBotToken")!;

            config.LogFormat = (message, exception) => $"{message.Source}: {message.Message}";
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
            services.AddHostedService<BotStatusService>();
        }).Build();

    await host.RunAsync();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}