using System.Runtime.Serialization;
using MediatR;
using Ordering.Application.Responses;

namespace Ordering.Application.Commands.OrderPaymentReject
{
    public class OrderPaymentRejectCommand: IRequest<CommandResult>
    {
        // TODO: Only one command class enough for a change of order status 

        [DataMember]
        public string OrderId { get; set; }

        [DataMember]
        public Domain.Enums.OrderStatus OrderStatus { get; set; }

        public OrderPaymentRejectCommand(string orderId, Domain.Enums.OrderStatus orderStatus)
        {
            OrderId = orderId;
            OrderStatus = orderStatus;
        }

    }
}