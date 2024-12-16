using OrderManager.Domain.Interfaces;

namespace OrderManager.Domain.Events
{

    public class OrderStateChangedEvent : IDomainEvent
    {
        public Guid OrderId { get; set; }
        public string FromState { get; set; }
        public string ToState { get; set; }
        public Guid StaffId { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime OccurredOn => Timestamp;
    }

}
