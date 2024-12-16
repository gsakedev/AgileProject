using MediatR;

namespace OrderManager.Application.Commands
{
    public class UpdateDeliveryStatusCommand : IRequest
    {
        public Guid OrderId { get; set; }
        public Guid StaffId { get; set; }
        public bool Delivered { get; set; }

    }
}
