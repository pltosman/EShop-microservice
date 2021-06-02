using System;
using System.Runtime.Serialization;
using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Commands.OrderStatus
{
    public class OrderStatusCommand : IRequest<CommandResult>
    {
        [DataMember]
        public Guid OrderId { get; set; }

        [DataMember]
        public Domain.Enums.OrderStatus OrderStatus { get; set; }

        [DataMember]
        public DateTime CreatedAt { get; set; }


        public OrderStatusCommand(Guid orderId, DateTime createdAt, Domain.Enums.OrderStatus status)
        {
            this.OrderId = orderId;
            this.CreatedAt = createdAt;
            this.OrderStatus = status;

        }

    }

}