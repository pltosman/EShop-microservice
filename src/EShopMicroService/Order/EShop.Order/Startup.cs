using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EShop.EventBus;
using EShop.EventBus.Abstractions;
using EShop.EventBusRabbitMQ;
using EShop.Order.IntegrationEvents.EventHandling;
using IntegrationEvents.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ordering.Application;
using Ordering.Application.AutofacModules;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Helpers;
using Ordering.Infrastructure.Middlewares;
using RabbitMQ.Client;
using Serilog;

namespace Order.EShop.Order
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            #region Add Infrastructure

            services.AddInfrastructure(Configuration);
            services.AddApplication();
            services.RegisterEventBus(Configuration);
            services.AddSingleton<IIntegrationEventHandler<OrderStatusEvent>, OrderStatusEventHandler>();
            #endregion
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EShop.Order", Version = "v1" });
            });

            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new MediatorModule());
            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EShop.Order v1"));

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

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


            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<OrderStatusEvent, IIntegrationEventHandler<OrderStatusEvent>>();
        }
    }


    public static class CustomExtensionsMethods
    {
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
            services.AddSingleton<IEventBus,EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                int retryCount = appSettings.RabbitMQSettings.RetryCount;

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }
    }
}
