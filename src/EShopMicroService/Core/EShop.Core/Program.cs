using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Serilog;
using System;
using Serilog.Events;
using Serilog.Exceptions;
using EShop.Core.Infrastructure.Enrichers;

namespace EShop.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                     .MinimumLevel.Verbose()
                     .Enrich.FromLogContext()
                     .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
                     .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
                     .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                     .MinimumLevel.Override("System", LogEventLevel.Information)
                     .Enrich.WithProperty("AppName", "Identity")
                     .Enrich.With<ThreadIdEnricher>()
                     .Enrich.WithExceptionDetails()
                     .WriteTo.Seq("http://127.0.0.1:8081")
                     .CreateLogger();
            try
            {
                Log.Information("Host starting.");

                var webHost = BuildWebHost(args);
                webHost.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .CaptureStartupErrors(true)
            .UseUrls("http://localhost:5001")
            .UseStartup<Startup>()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureLogging(options =>
            {
                options.ClearProviders();
            })
            .UseSerilog()
            .Build();
    }
}
