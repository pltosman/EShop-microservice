using FluentValidation;

namespace Ordering.Application.Commands.OrderStatus
{
    public class OrderStatusValidator : AbstractValidator<OrderStatusCommand>
    {
        public OrderStatusValidator()
        {
            RuleFor(v => v.OrderId).NotEmpty();
            RuleFor(v => v.OrderStatus).NotEmpty();
        }
    }
}
