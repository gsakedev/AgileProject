using OrderManager.Domain.Entities;
using OrderManager.Domain.Enums;
using OrderManager.Domain.States;

namespace OrderManager.Domain.Factories
{
    public class OrderStateFactory
    {
        public static OrderStateBase CreateState(Order order)
        {
            return order.State switch
            {
                OrderState.Pending => new PendingState(order),
                OrderState.Preparing => new PreparingState(order),
                OrderState.ReadyForDelivery => new ReadyForDeliveryState(order),
                OrderState.OutForDelivery => new OutForDeliveryState(order),
                OrderState.Delivered => new DeliveredState(order),
                OrderState.ReadyForPickup => new ReadyForPickupState(order),
                OrderState.UnableToDeliver => new UnableToDeliverState(order),
                _ => throw new InvalidOperationException($"Unknown state: {order.State}")
            };
        }
    }
}
