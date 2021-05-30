using System;
using EShop.EventBus.Events;

namespace IntegrationEvents.Events
{
    public class ShipmentEvent : IntegrationEvent
    {
        public string MerchantName { get; set; }
        public Guid  OrderId { get; set; }
        public decimal TotalPrice { get; set; }

        public ShipmentEvent(string merchantName, Guid orderId, decimal totalPrice)
        {
            MerchantName = merchantName;
            OrderId = orderId;
            TotalPrice = totalPrice;
        }
    }
}
