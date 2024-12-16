using OrderManager.Domain.Entities;
using OrderManager.Domain.Enums;

namespace OrderManager.Domain.Specifications
{
    public class DeliveredOrdersSpecification : BaseSpecification<Order>
    {
        public DeliveredOrdersSpecification(DateTime startDate, DateTime endDate, Guid? staffId = null)
            : base(o => o.State == OrderState.Delivered &&
                        o.CompletedDate.HasValue &&
                        o.OrderDate >= startDate &&
                        o.CompletedDate.Value <= endDate &&
                        o.State.Equals(OrderState.Delivered))
        { }
    }
}
