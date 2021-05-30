using System;
using System.Runtime.Serialization;
using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Commands.OrderPaymentSuccess
{
    public class OrderPaymentSuccessCommand : IRequest<CommandResult>
    {
        // TODO: Only one command class enough for a change of order status 

        [DataMember]
        public Guid OrderId { get; set; }

        [DataMember]
        public Domain.Enums.OrderStatus OrderStatus { get; set; }

        public OrderPaymentSuccessCommand(Guid orderId, Domain.Enums.OrderStatus orderStatus)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
        }

    }
}