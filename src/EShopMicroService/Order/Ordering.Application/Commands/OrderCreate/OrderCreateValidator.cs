using FluentValidation;

namespace Ordering.Application.Commands.OrderCreate
{
    public class OrderCreateValidator : AbstractValidator<OrderCreateCommand>
    {
        public OrderCreateValidator()
        {
            RuleFor(v => v.MerchantName).NotEmpty();
            RuleFor(v => v.ProductId).NotEmpty();
        }
    }
}