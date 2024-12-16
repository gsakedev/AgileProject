using OrderManager.Domain.Entities;
using OrderManager.Domain.Enums;

namespace OrderManager.Domain.States
{
    public abstract class OrderStateBase
    {
        protected Order Order { get; }

        protected OrderStateBase(Order order)
        {
            Order = order;
        }
        public abstract void MoveToNext();
        public abstract void Cancel();

        protected void LogTransition(OrderState newState)
        {
            Console.WriteLine($"Order {Order.Id}: Transitioning from {Order.State} to {newState}");
        }
    }
}
