using MediatR;

namespace OrderManager.Application.Notifications
{
    public class OrderStateChangedNotification : INotification
    {
        public Guid OrderId { get; set; }
        public string FromState { get; set; }
        public string ToState { get; set; }
        public Guid StaffId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
