using MediatR;

namespace OrderManager.Application.Commands
{
    public class ChangeDeliveryStaffStatusCommand : IRequest
    {
        public string DeliveryStaffId { get; set; }
        public Guid? OrderId { get; set; } 
        public bool IsAvailable { get; set; }
    }
}
