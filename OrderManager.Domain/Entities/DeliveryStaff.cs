using OrderManager.Domain.Interfaces;

namespace OrderManager.Domain.Entities
{
    public class DeliveryStaff
    {
        public string Id { get; set; } 
        public bool IsAvailable { get; set; } = true;
        public string? CurrentLocation { get; set; } 
        public Guid? CurrentOrderId { get; set; } 
        public Order? CurrentOrder { get; set; }
        public IUser User { get; set; } 
    }
}
