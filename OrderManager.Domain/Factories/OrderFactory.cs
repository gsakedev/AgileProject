using OrderManager.Domain.Entities;
using OrderManager.Domain.Enums;
using OrderManager.Domain.Interfaces;
using OrderManager.Domain.States;


namespace OrderManager.Domain.Factories
{
    public class OrderFactory : IOrderFactory
    {
        public Order CreateOrder(Guid customerId, List<OrderItem> items, DeliveryOption deliveryOption, string? instructions)
        {
            var totalPrice = items.Sum(i => i.Price * i.Quantity);

            var order = new Order
            {
                CustomerId = customerId,
                DeliveryOption = deliveryOption,
                SpecialInstructions = instructions,
                State = OrderState.Pending,
                Items = items,
                OrderDate = DateTime.UtcNow,
                OrderTotalPrice = totalPrice
            };
            order.OrderStatesB = new PendingState(order);
            return order;

        }

    }
}
