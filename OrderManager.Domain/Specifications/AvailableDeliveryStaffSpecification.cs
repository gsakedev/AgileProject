using OrderManager.Domain.Entities;

namespace OrderManager.Domain.Specifications
{
    public class AvailableDeliveryStaffSpecification : BaseSpecification<DeliveryStaff>
    {
        public AvailableDeliveryStaffSpecification()
        : base(d => d.IsAvailable)
        {
            AddInclude(d => d.CurrentOrder); // Include related orders
        }
    }
}
