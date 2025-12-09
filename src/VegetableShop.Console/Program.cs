using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using VegetableShop.Console.Extensions;
using VegetableShop.Console.Services;

namespace VegetableShop.Console
{
    /// <summary>
    /// Main entry point for the Vegetable Shop application.
    /// </summary>
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();
                var app = host.Services.GetRequiredService<IVegetableShopApplication>();

                return (int)await app.RunAsync(args);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application could not start.");
                return (int)ExitCodes.UnexpectedError;
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }

        /// <summary>
        /// Creates and configures the host builder.
        /// </summary>
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddCommandLine(args);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddApplicationServices(context.Configuration);
                })
                .UseSerilog((context, services, loggerConfiguration) =>
                {
                    var logPath = context.Configuration["Logging:FilePath"] ?? "logs/log.txt";
                    
                    loggerConfiguration
                        .ReadFrom.Configuration(context.Configuration)
                        .WriteTo.File(logPath, rollingInterval: RollingInterval.Day);
                });
    }
}