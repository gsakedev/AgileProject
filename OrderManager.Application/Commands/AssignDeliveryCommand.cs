using MediatR;

namespace OrderManager.Application.Commands
{
    public class AssignDeliveryCommand : IRequest
    {
        public Guid OrderId { get; set; }
        public Guid DeliveryStaffId { get; set; }

    }
}
