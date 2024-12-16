using FluentValidation;
using OrderManager.Domain.DTOs;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Application.Validators
{
    public class PlaceOrderBusinessValidator : AbstractValidator<PlaceOrderDTO>
    {

        private readonly IMenuItemRepository _menuItemRepository;

        public PlaceOrderBusinessValidator(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;

            RuleForEach(x => x.Items).MustAsync(MenuItemExists).WithMessage("MenuItemId does not exist.");
        }

        private async Task<bool> MenuItemExists(OrderItemDto item, CancellationToken cancellationToken)
        {
            return await _menuItemRepository.ExistsAsync(m => m.Id == item.MenuItemId, cancellationToken);
        }
    }
}
