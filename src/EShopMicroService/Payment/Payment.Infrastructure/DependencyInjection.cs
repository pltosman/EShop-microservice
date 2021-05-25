using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Domain.Repositories;
using Payment.Domain.Repositories.Base;
using Payment.Infrastructure.Data;
using Payment.Infrastructure.Repositories;
using Payment.Infrastructure.Repositories.Base;

namespace Payment.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PaymentContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"),
                                               ServiceLifetime.Singleton,
                                               ServiceLifetime.Singleton);

            // services.AddDbContext<PaymentContext>(options =>
            //         options.UseSqlServer(
            //             configuration.GetConnectionString("PaymentConnection"),
            //             b => b.MigrationsAssembly(typeof(PaymentContext).Assembly.FullName)), ServiceLifetime.Singleton);

            //Add Repositories
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IPaymentRepository, PaymentRepository>();

            return services;
        }
    }
}
