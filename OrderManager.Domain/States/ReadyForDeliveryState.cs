using OrderManager.Domain.Entities;
using OrderManager.Domain.Enums;
using OrderManager.Domain.Exceptions;

namespace OrderManager.Domain.States
{
    public class ReadyForDeliveryState : OrderStateBase
    {
        public ReadyForDeliveryState(Order order) : base(order) { }

        public override void MoveToNext()
        {
            LogTransition(OrderState.OutForDelivery);

            Order.State = OrderState.OutForDelivery;
            Order.OrderStatesB = new OutForDeliveryState(Order);
        }

        public override void Cancel()
        {
            throw new OrderStateException("Cannot cancel an order that is ready for delivery.");
        }
    }
}
