using MediatR;
using OrderManager.Application.Commands;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Application.Handlers
{
    public class PlaceOrderHandler : IRequestHandler<PlaceOrderCommand, Guid>
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IOrderFactory _orderFactory;
        private readonly IUserContextService _userContextService;

        public PlaceOrderHandler(IRepository<Order> orderRepository, IMenuItemRepository menuItemRepository, IOrderFactory orderFactory,
            IUserContextService userContextService)
        {
            _orderRepository = orderRepository;
            _menuItemRepository = menuItemRepository;
            _orderFactory = orderFactory;
            _userContextService = userContextService;
        }
        public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            request.Items.ForEach(item => item.Quantity = item.Quantity > 0 ? item.Quantity : 1);
            var customerId = _userContextService.GetCurrentUserId();

            // Fetch and validate MenuItems
            var menuItems = await _menuItemRepository.GetAvailableMenuItemsAsync(request.Items.Select(oi => oi.MenuItemId));
            if ((menuItems is null) || menuItems.Count != request.Items.Count)
            {
                throw new ArgumentException("Some MenuItemIds are invalid or unavailable.");
            }

            // Map OrderItems to MenuItems
            var orderItems = request.Items.Select(oi =>
            {
                var menuItem = menuItems.First(mi => mi.Id == oi.MenuItemId);

                return new OrderItem
                {
                    MenuItemId = menuItem.Id,
                    Quantity = oi.Quantity,
                    Name = menuItem.Name,
                    Price = menuItem.Price
                };
            }).ToList();

            // Create the order
            var order = _orderFactory.CreateOrder(
                customerId,
                orderItems,
                request.DeliveryOption,
                request.SpecialInstructions
            );
            order.CalculateTotalPrice();

            await _orderRepository.AddAsync(order, cancellationToken);
            await _orderRepository.SaveChangesAsync(cancellationToken);

            return order.Id;

        }
    }
}
