using MediatR;
using Ordering.Application.Responses;
using System;
using System.Runtime.Serialization;

namespace Ordering.Application.Commands.OrderCreate
{
    public class OrderCreateCommand : IRequest<OrderResponse>
    {
        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public decimal TotalPrice { get; set; }

        [DataMember]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        public string MerchantName { get; set; }

        public OrderCreateCommand(string productId, decimal totalPrice, DateTime createdAt, string merchantName)
        {
            this.ProductId = productId;
            this.TotalPrice = totalPrice;
            this.CreatedAt = createdAt;
            this.MerchantName = merchantName;

        }

    }

}