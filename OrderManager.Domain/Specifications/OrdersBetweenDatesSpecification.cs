using OrderManager.Domain.Entities;

namespace OrderManager.Domain.Specifications
{
    public class OrdersBetweenDatesSpecification : BaseSpecification<Order>
    {
        public OrdersBetweenDatesSpecification(DateTime startDate, DateTime endDate)
            : base(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
        {
            AddOrderBy(o => o.OrderDate);
        }
    }
}
