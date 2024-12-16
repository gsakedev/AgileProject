using OrderManager.Application.Notifications;
using OrderManager.Domain.Events;

namespace OrderManager.Application.Mappers
{
    public static class DomainEventMapper
    {
        public static OrderStateChangedNotification ToNotification(OrderStateChangedEvent domainEvent)
        {
            return new OrderStateChangedNotification
            {
                OrderId = domainEvent.OrderId,
                FromState = domainEvent.FromState,
                ToState = domainEvent.ToState,
                StaffId = domainEvent.StaffId,
                Timestamp = domainEvent.Timestamp
            };
        }
        public static DeliveryStaffStatusChangeNotification ToNotification(DeliveryStaffStatusChangeEvent domainEvent)
        {
            return new DeliveryStaffStatusChangeNotification
            {
                OrderId = domainEvent.OrderId,
                Timestamp = domainEvent.Timestamp,
                DeliveryStaffId = domainEvent.DeliveryStaffId,
                IsAvailable = domainEvent.IsAvailable
            };
        }
    }
}
