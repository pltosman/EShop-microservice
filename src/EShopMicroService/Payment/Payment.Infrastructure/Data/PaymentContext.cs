using System;
using Microsoft.EntityFrameworkCore;

namespace Payment.Infrastructure.Data
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
        {

        }

        public DbSet<Domain.Entities.Payment> Payments { get; set; }
    }
}
