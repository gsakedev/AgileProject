using FluentValidation;
using OrderManager.Domain.DTOs;

namespace OrderManager.Application.Validators
{
    public class CreateMenuItemDTOValidator : AbstractValidator<MenuItemDTO>
    {
        public CreateMenuItemDTOValidator()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(dto => dto.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(dto => dto.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

        }
    }
}
