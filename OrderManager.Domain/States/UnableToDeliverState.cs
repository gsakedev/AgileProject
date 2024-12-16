using OrderManager.Domain.Entities;
using OrderManager.Domain.Exceptions;

namespace OrderManager.Domain.States
{
    public class UnableToDeliverState : OrderStateBase
    {
        public UnableToDeliverState(Order order) : base(order)
        {
            order.IsCompleted = true;
        }
        public override void Cancel()
        {
            throw new OrderStateException("UnableToDeliver orders cannot be canceled.");
        }

        public override void MoveToNext()
        {
            throw new OrderStateException("UnableToDeliver orders cannot transition to another state.");
        }
    }
}
