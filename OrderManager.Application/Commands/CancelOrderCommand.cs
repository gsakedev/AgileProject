using MediatR;

namespace OrderManager.Application.Commands
{
    public class CancelOrderCommand : IRequest
    {
        public Guid OrderId { get; set; }
        public Guid StaffId { get; set; }
    }
}
