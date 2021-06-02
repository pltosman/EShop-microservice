using System;
using System.Runtime.Serialization;
using MediatR;
using Payment.Application.Responses;
using Payment.Domain.Entities;

namespace Payment.Application.Commands.CreatePayment
{
    public class PaymentCreateCommand : IRequest<PaymentResponse>
    {
        [DataMember]
        public Guid OrderId { get; set; }

        [DataMember]
        public string CustomerId { get; set; }

        [DataMember]
        public Card Card { get; set; }

        public PaymentCreateCommand(Guid orderId, string customerId, Card card)
        {
            OrderId = orderId;
            CustomerId = customerId;
            Card = card;
        }
    }
}
