using System;
using EShop.Core.Domain.Entities;
using EShop.Core.Domain.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace EShop.Core.Infrastructure.Data
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ClientRequestEntityTypeConfiguration());
            builder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
            builder.ApplyConfiguration(new CustomerDetailEntityTypeConfiguration());
            builder.ApplyConfiguration(new CustomerTokenEntityTypeConfiguration());
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerDetail> CustomerDetails { get; set; }
        public DbSet<CustomerToken> CustomerTokens { get; set; }
       
    }
}
