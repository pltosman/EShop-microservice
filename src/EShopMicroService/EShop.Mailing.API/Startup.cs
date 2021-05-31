using EShop.EventBus.Abstractions;
using EShop.Mailing.API.Helpers;
using EShop.Mailing.API.IntegrationEvents.EventHandling;
using EShop.Mailing.API.IntegrationEvents.Events;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using EShop.EventBus;
using EShop.EventBusRabbitMQ;
using Microsoft.OpenApi.Models;

namespace EShop.Mailing.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EShop.Mailing.API", Version = "v1" });
            });
            services
                .AddCustomMvc()
                .RegisterEventBus(Configuration)
                .AddEvents();

            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EShop.Mailing.API v1"));
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<EmailConfirmationEvent, IIntegrationEventHandler<EmailConfirmationEvent>>();

            app.UseRouting();

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
                services.AddControllers();

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

            public static IServiceCollection AddEvents(this IServiceCollection services)
            {
                services.AddSingleton<IIntegrationEventHandler<EmailConfirmationEvent>, EmailConfirmationEventHandler>();
             

                return services;
            }
        }
    }
