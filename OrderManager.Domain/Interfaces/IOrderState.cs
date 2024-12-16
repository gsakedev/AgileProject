using OrderManager.Domain.Entities;

namespace OrderManager.Domain.Interfaces
{
    public interface IOrderState
    {
        void MoveToNextState(Order order);
        void CancelOrder(Order order);
    }
}
