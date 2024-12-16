using OrderManager.Domain.Enums;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Domain.Events
{
    public class OrderReadyForDeliveryEvent : IDomainEvent
    {
        public Guid OrderId { get; }
        public DeliveryOption DeliveryOption { get; }
        public DateTime OccurredOn => DateTime.UtcNow;

        public OrderReadyForDeliveryEvent(Guid orderId, DeliveryOption deliveryOption)
        {
            OrderId = orderId;
            DeliveryOption = deliveryOption;
        }
    }
}
