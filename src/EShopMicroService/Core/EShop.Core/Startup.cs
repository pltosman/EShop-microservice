using EShop.EventBus;
using EShop.EventBus.Abstractions;
using EShop.EventBusRabbitMQ;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EShop.Core.Infrastructure.Data;
using EShop.Core.Infrastructure.AutofacModules;
using EShop.Core.Infrastructure.Filters;
using EShop.Core.Helpers;
using EShop.Core.Application.PipelineBehaviours;
using EShop.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Microsoft.Extensions.Hosting;
using EShop.Core.Model.ResponseModels;
using EShop.Core.Model.Enums;

namespace EShop.Core
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddSingleton<IAccountService, AccountService>();

            services
                .AddCustomMvc()
                .AddCustomDbContext()
                .AddCustomDistributedCache()
                .AddCustomJWTToken(Configuration)
                .RegisterEventBus(Configuration);


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EShop.Core", Version = "v1" });
            });

            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new MediatorModule());

            return new AutofacServiceProvider(container.Build());

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EShop.Core v1"));
            }

            var forwardingOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };

            forwardingOptions.KnownNetworks.Clear();
            forwardingOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardingOptions);

            app.UseMiddleware<HttpContextLoggingMiddleware>();

            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = LogHelper.EnrichFromRequest;
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }



    static class CustomExtensionsMethods
    {

        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                })
                .AddNewtonsoftJson();

            return services;
        }



        public static IServiceCollection AddCustomDbContext(this IServiceCollection services)
        {
            services
                            .AddDbContext<IdentityContext>(options =>
                            {
                                options.UseNpgsql("User ID=hlzsocfqeqojgt;Password=feb747e1f7ba3c3b3c23747e43720d150743d9027689b281af86293799b288c8;Host=ec2-3-217-219-146.compute-1.amazonaws.com;Port=5432;Database=d6gthpuo977f07;sslmode=Prefer;Trust Server Certificate=true;Pooling=true;");
                            });

            return services;
        }

        public static IServiceCollection AddCustomJWTToken(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "bearer";
                    options.DefaultChallengeScheme = "bearer";
                })
                .AddJwtBearer("bearer", options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.Audience = appSettings.TokenSettings.JwtTokenAudience;
                    options.ClaimsIssuer = appSettings.TokenSettings.JwtTokenIssuer;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.TokenSettings.Secret)),
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                var response = new CommandResult
                                {
                                    Status = ResponseStatus.Unauthorized,
                                    Message = "Token expired !"
                                };

                                context.Response.StatusCode = (int)HttpStatusCode.OK;
                                context.Response.ContentType = "application/json charset=utf-8";

                                return context.Response.WriteAsJsonAsync(response);
                            }

                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            string customerId = ((ClaimsIdentity)context.Principal.Identity).Claims
                                .Where(x => x.Type == "CustomerId")
                                .Select(x => x.Value)
                                .FirstOrDefault();

                            var claims = new List<Claim>
                            {
                                new Claim("CustomerId",customerId)
                            };

                            var appIdentity = new ClaimsIdentity(claims);
                            context.Principal.AddIdentity(appIdentity);

                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }

        public static IServiceCollection AddCustomDistributedCache(this IServiceCollection services)
        {
            services
                .AddDistributedRedisCache(options =>
                {
                    options.InstanceName = "Identity";
                    options.Configuration = "ec2-18-233-139-31.compute-1.amazonaws.com:8900,password=p09369f3ebf884e2007fe1443aad6128e29d2266c627b63002f5c4011b8a745fd";
                });

            return services;
        }

        public static IServiceCollection RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = appSettings.RabbitMQSettings.Connection,
                    DispatchConsumersAsync = true
                };

                factory.UserName = appSettings.RabbitMQSettings.UserName;
                factory.Password = appSettings.RabbitMQSettings.Password;

                int retryCount = appSettings.RabbitMQSettings.RetryCount;

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            var subscriptionClientName = appSettings.RabbitMQSettings.SubscriptionClientName;
            services.AddSingleton<IEventBus, EventBusRabbitMQ.EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ.EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                int retryCount = appSettings.RabbitMQSettings.RetryCount;

                return new EventBusRabbitMQ.EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }

    }
}


