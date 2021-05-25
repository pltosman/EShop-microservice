using System;
using FluentValidation;

namespace Payment.Application.Commands.CreatePayment
{
    public class PaymentCreateValidator : AbstractValidator<PaymentCreateCommand>
    {
        public PaymentCreateValidator()
        {
            RuleFor(v => v.OrderId).NotEmpty();
            RuleFor(v => v.Card.CCV).NotEmpty().Length(3);
            RuleFor(v => v.Card.LongNum).NotEmpty().Length(16);
            RuleFor(v => v.Card.Expires).NotEmpty();
          
        }
    }
}
