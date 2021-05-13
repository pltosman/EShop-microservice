using MediatR;
using Ordering.Application.Responses;
using System;

namespace Ordering.Application.Commands.OrderCreate
{
    public class OrderCreateCommand : IRequest<OrderResponse>
    {
        public string ProductId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SellerUserName { get; set; }




        
    }

}