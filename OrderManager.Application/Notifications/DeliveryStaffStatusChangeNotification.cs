using MediatR;

namespace OrderManager.Application.Notifications
{
    public class DeliveryStaffStatusChangeNotification : INotification
    {
        public Guid? OrderId { get; set; }
        public string DeliveryStaffId { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
