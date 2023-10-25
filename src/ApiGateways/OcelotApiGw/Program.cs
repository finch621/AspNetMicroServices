using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

builder
    .Host
    .ConfigureAppConfiguration((host, config) =>
    {
        config.AddJsonFile($"ocelot.{host.HostingEnvironment.EnvironmentName}.json", true, true);
    })
    .ConfigureLogging((host, logger) =>
    {
        logger.AddConfiguration(host.Configuration.GetSection("Logging"));
        logger.AddConsole();
        logger.AddDebug();
    });

builder
    .Services
    .AddOcelot()
    .AddCacheManager(c =>
    {
        c.WithDictionaryHandle();
    });

var app = builder.Build();

await app.UseOcelot();

app.MapGet("/", () => "Hello World!");

app.Run();
