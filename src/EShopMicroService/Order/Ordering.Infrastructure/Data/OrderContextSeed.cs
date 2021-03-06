using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());

                await orderContext.SaveChangesAsync();
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>()
            {
               new Order()
               {
                   ProductId = Guid.NewGuid().ToString(),
                   MerchantName = "Apple",
                   TotalPrice = 1000,
                   CreatedAt = DateTime.UtcNow
               },
               new Order()
               {
                   ProductId = Guid.NewGuid().ToString(),
                   MerchantName = "Amazon",
                   TotalPrice = 1000,
                   CreatedAt = DateTime.UtcNow
               },
               new Order()
               {
                   ProductId = Guid.NewGuid().ToString(),
                   MerchantName = "Amazon",
                   TotalPrice = 1000,
                   CreatedAt = DateTime.UtcNow
               }
            };
        }
    }
}
