using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SenderService.Extensions;
using Serilog;
using Winton.Extensions.Configuration.Consul;

namespace SenderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfiguration configuration = LoadConfiguration(environment);
            ConfigureLogging(configuration, environment);
            CreateHostBuilder(configuration).Build().RunReceiveService().Run();
        }

        public static IHostBuilder CreateHostBuilder(IConfiguration configuration) =>
            Host.CreateDefaultBuilder()
                .UsePrometheusMetrics()
                .ConfigureAppConfiguration(
                    (hostingContext, builder) =>
                    {
                        IConfigurationBuilder config = builder
                            .AddConsul(
                                "git_config/SenderService/appsettings.json",
                                options =>
                                {
                                    options.ConsulConfigurationOptions = cco =>
                                    {
                                        cco.Address = new Uri(
                                            configuration["ConsulConfiguration:Uri"]
                                        );
                                    };
                                    options.Optional = true;
                                    options.ReloadOnChange = true;
                                    options.OnLoadException = exceptionContext =>
                                    {
                                        exceptionContext.Ignore = true;
                                    };
                                }
                            )
                            .AddEnvironmentVariables();
                    }
                )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseKestrel()
                        .UseSerilog()
                        .UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS"))
                        .UseStartup<Startup>();
                });

        private static void ConfigureLogging(IConfiguration configuration, string environment)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich
                .WithProperty("Environment", environment)
                .ReadFrom
                .Configuration(configuration)
                .CreateLogger();
        }

        private static IConfiguration LoadConfiguration(string environment)
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();
        }
    }
}
