using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EShop.Order.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Enrichers;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace Order.EShop.Order
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
                     .Enrich.WithProperty("AppName", "Order")
                     .Enrich.With<ThreadIdEnricher>()
                     .Enrich.WithExceptionDetails()
                     .WriteTo.Seq("http://127.0.0.1:8081", apiKey: "zWj6SS0b5Vp1GTM95jDp")
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
            .UseUrls("http://localhost:5003")
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


