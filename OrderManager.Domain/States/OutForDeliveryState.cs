using OrderManager.Domain.Entities;
using OrderManager.Domain.Enums;
using OrderManager.Domain.Exceptions;

namespace OrderManager.Domain.States
{
    public class OutForDeliveryState : OrderStateBase
    {
        public OutForDeliveryState(Order order) : base(order) { }

        public override void MoveToNext()
        {
            throw new InvalidOperationException("Cannot transition directly from OutForDelivery. Use explicit methods.");
        }

        public override void Cancel()
        {
            throw new OrderStateException("Cannot cancel an order that is ready for delivery.");
        }

        public void MarkAsDelivered()
        {
            LogTransition(OrderState.Delivered);

            Order.State = OrderState.Delivered;
            Order.OrderStatesB = new DeliveredState(Order);
        }

        public void MarkAsUnableToDeliver()
        {
            LogTransition(OrderState.UnableToDeliver);

            Order.State = OrderState.UnableToDeliver;
            Order.OrderStatesB = new UnableToDeliverState(Order);
        }
    }
}
