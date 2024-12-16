using OrderManager.Domain.Entities;
using OrderManager.Domain.Enums;

namespace OrderManager.Domain.States
{
    public class PreparingState : OrderStateBase
    {
        public PreparingState(Order order) : base(order) { }

        public override void MoveToNext()
        {
            if (Order.DeliveryOption == DeliveryOption.Pickup)
            {
                LogTransition(OrderState.ReadyForPickup);

                Order.State = OrderState.ReadyForPickup;
                Order.OrderStatesB = null;
            }
            else
            {
                LogTransition(OrderState.ReadyForDelivery);

                Order.State = OrderState.ReadyForDelivery;
                Order.OrderStatesB = new ReadyForDeliveryState(Order);
            }
        }

        public override void Cancel()
        {
            LogTransition(OrderState.Cancelled);

            Order.State = OrderState.Cancelled;
            Order.OrderStatesB = null; // Terminal state
        }
    }
}
