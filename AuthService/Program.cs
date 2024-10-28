using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using Core.Extensions;

namespace AuthService
{
    public class Program
	{
		private static IConfigurationRoot _configuration;

		public static void Main(string[] args)
		{
			ConfigureLogging();
			string currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			_configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddJsonFile($"appsettings.{currentEnv}.json", optional: true)
				.AddUserSecrets<Program>()
				.Build();

			var host = Host.CreateDefaultBuilder(args)
                .UsePrometheusMetrics()
				.ConfigureWebHostDefaults(builder =>
				{
					builder
						.UseConfiguration(_configuration)
						.UseKestrel()
						.UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS"))
						.UseContentRoot(Directory.GetCurrentDirectory())
						.UseSerilog()
						.UseShutdownTimeout(TimeSpan.FromSeconds(50))
						.UseStartup<Startup>();
				});

			try
			{
				Log.Information("Starting web host");
				host.Build().Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Web Host terminated unexpectedly");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}
		
		private static void ConfigureLogging()
		{
			var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile(
					$"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
					optional: true)
				.Build();

			Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("Environment", environment)
				.ReadFrom.Configuration(configuration)
				.CreateLogger();
		}
    }
}
