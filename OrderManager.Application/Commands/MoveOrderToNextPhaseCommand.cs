using MediatR;

namespace OrderManager.Application.Commands
{
    public class MoveOrderToNextPhaseCommand : IRequest
    {
        public Guid OrderId { get; set; }
        public Guid StaffId { get; set; }
    }
}
