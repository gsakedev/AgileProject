using OrderManager.Domain.Entities;
using OrderManager.Domain.Exceptions;

namespace OrderManager.Domain.States
{
    public class ReadyForPickupState : OrderStateBase
    {
        public ReadyForPickupState(Order order) : base(order)
        {
            order.IsCompleted = true;
            order.CompletedDate = DateTime.Now;
        }
        public override void Cancel()
        {
            throw new OrderStateException("ReadyForPickupState orders cannot be canceled.");
        }

        public override void MoveToNext()
        {
            throw new OrderStateException("ReadyForPickupState orders cannot transition to another state.");
        }
    }
}
