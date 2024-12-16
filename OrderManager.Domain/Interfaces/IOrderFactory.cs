using OrderManager.Domain.Entities;
using OrderManager.Domain.Enums;

namespace OrderManager.Domain.Interfaces
{
    public interface IOrderFactory
    {
        Order CreateOrder(Guid customerId, List<OrderItem> items, DeliveryOption deliveryOption, string? instructions);

    }
}
