namespace OrderManager.Domain.Entities
{
    public class OrderStateAudit
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; } // Reference to the order
        public string FromState { get; set; } // Previous state
        public string ToState { get; set; } // New state
        public Guid StaffId { get; set; } // Who made the change
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // When it happened
    }
}
