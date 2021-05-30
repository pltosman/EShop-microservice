using System.Runtime.Serialization;
using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Commands.OrderDelivered
{
    public class OrderDeliveredCommand : IRequest<OrderResponse>
    {
        // TODO: Only one command class enough for a change of order status 

        [DataMember]
        public string OrderId { get; set; }

        [DataMember]
        public Domain.Enums.OrderStatus OrderStatus { get; set; }

        public OrderDeliveredCommand(string orderId, Domain.Enums.OrderStatus orderStatus)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
        }

    }
}