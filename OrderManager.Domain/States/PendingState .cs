using OrderManager.Domain.Entities;
using OrderManager.Domain.Enums;

namespace OrderManager.Domain.States
{

    public class PendingState : OrderStateBase
    {
        public PendingState(Order order) : base(order) { }

        public override void MoveToNext()
        {
            LogTransition(OrderState.Preparing);

            Order.State = OrderState.Preparing;
            Order.OrderStatesB = new PreparingState(Order);
        }

        public override void Cancel()
        {
            LogTransition(OrderState.Cancelled);

            Order.State = OrderState.Cancelled;
            Order.OrderStatesB = null; // Terminal state
        }
    }

}
