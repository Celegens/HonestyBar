using System;
using System.IO;
using System.Reflection;
using HonestyBar.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;

namespace HonestyBar
{
    public static class Program
    {
        public static IConfiguration Configuration { get; } = BuildConfiguration();

        public static void Main(string[] args) => Run(args);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Application should return specific error code since exception is logged")]
        public static int Run(string[] args)
        {
            Log.Logger = BuildLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Web host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        private static Logger BuildLogger() =>
            new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.WithProperty("Application", GetAssemblyProductName())
                .CreateLogger();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .CaptureStartupErrors(true)
                        .UseConfiguration(Configuration)
                        .ConfigureAppConfiguration((hostingContext, config) =>
                            AddConfiguration(config, hostingContext.HostingEnvironment, args))
                        .UseSerilog()
                        .UseAzureAppServices()
                        .UseStartup<Startup>();
                });

        private static IConfigurationBuilder AddConfiguration(IConfigurationBuilder config,
            IWebHostEnvironment hostingEnvironment, string[] args)
            => config
                // Add configuration specific to the Development, Staging or Production environments. This config can
                // be stored on the machine being deployed to or if you are using Azure, in the cloud. These settings
                // override the ones in all of the above config files.
                .AddEnvironmentVariables()
                // Push telemetry data through the Azure Application Insights pipeline faster in the development and
                // staging environments, allowing you to view results immediately.
                .AddApplicationInsightsSettings(developerMode: !hostingEnvironment.IsProduction())
                // This reads the configuration keys from the secret store. This allows you to store connection strings
                // and other sensitive settings, so you don't have to check them into your source control provider.
                // Only use this in Development, it is not intended for Production use. See
                // http://docs.asp.net/en/latest/security/app-secrets.html
                .AddIf(
                    hostingEnvironment.IsDevelopment(),
                    x => x.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true))
                // Add command line options. These take the highest priority.
                .AddIf(
                    args != null,
                    x => x.AddCommandLine(args));

        private static string GetAssemblyProductName() =>
            Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyProductAttribute>()?.Product;
    }
}
