using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EShop.Core.Infrastructure.Enrichers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

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
                     .WriteTo.Seq("http://locahost:8081")
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
                .CaptureStartupErrors(false)
                .ConfigureLogging(options =>
                {
                    options.ClearProviders();
                })
                .UseUrls("http://localhost:5001")
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseSerilog()
                .Build();
        }
    }
