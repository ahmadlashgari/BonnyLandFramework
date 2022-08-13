using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BL.Framework.AspNetCore
{
    public static class HostBuilder
    {
        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args, bool configLogger, bool openApiDocument) where TStartup : class
        {
            if (configLogger)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.Serilog.json", optional: false, reloadOnChange: true).Build();

                Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            }

            var app = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(config =>
                {
                    if (configLogger)
                    {
                        config.ClearProviders();
                        config.AddSerilog();
                    }
                })
                .ConfigureAppConfiguration(config =>
                {
                    if (openApiDocument)
                    {
                        config.AddJsonFile("appsettings.Swagger.json", false, true);
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    if (configLogger)
                    {
                        webBuilder.UseSerilog();
                    }

                    webBuilder.UseStartup<TStartup>();
                });

            return app;
        }
    }
}
