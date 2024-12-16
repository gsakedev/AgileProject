using OrderManager.Domain.Entities;
using OrderManager.Domain.Exceptions;

namespace OrderManager.Domain.States
{
    public class DeliveredState : OrderStateBase
    {
        public DeliveredState(Order order) : base(order)
        {
            order.IsCompleted = true;
            order.CompletedDate = DateTime.UtcNow;
        }
        public override void Cancel()
        {
            throw new OrderStateException("Delivered orders cannot be canceled.");
        }

        public override void MoveToNext()
        {
            throw new OrderStateException("Delivered orders cannot transition to another state.");
        }
    }
}
