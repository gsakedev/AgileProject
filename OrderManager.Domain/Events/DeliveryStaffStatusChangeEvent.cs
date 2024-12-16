using OrderManager.Domain.Interfaces;

namespace OrderManager.Domain.Events
{
    public class DeliveryStaffStatusChangeEvent : IDomainEvent
    {
        public Guid? OrderId { get; set; } 
        public string DeliveryStaffId { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime OccurredOn => Timestamp;

    }
}
