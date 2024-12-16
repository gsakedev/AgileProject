using FluentValidation;
using OrderManager.Domain.DTOs;
using OrderManager.Domain.Enums;

namespace OrderManager.Application.Validators
{
    public class PlaceOrderDTOValidator : AbstractValidator<PlaceOrderDTO>
    {
        public PlaceOrderDTOValidator()
        {


            RuleFor(x => x.DeliveryOption)
                .NotEmpty().WithMessage("DeliveryOption is required.")
                .IsEnumName(typeof(DeliveryOption), caseSensitive: false)
                .WithMessage("Invalid DeliveryOption provided.");

            RuleForEach(x => x.Items).ChildRules(orderItem =>
            {
                orderItem.RuleFor(x => x.MenuItemId)
                    .NotEmpty().WithMessage("MenuItemId is required.");
                orderItem.RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            });
        }
    }
}
