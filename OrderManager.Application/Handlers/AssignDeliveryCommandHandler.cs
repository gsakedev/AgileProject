using MediatR;
using Microsoft.AspNetCore.Http;
using OrderManager.Application.Commands;
using OrderManager.Domain.Enums;
using OrderManager.Domain.Interfaces;
using System.Security.Claims;

namespace OrderManager.Application.Handlers
{
    public class AssignDeliveryCommandHandler : IRequestHandler<AssignDeliveryCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDeliveryStaffRepository _deliveryStaffRepository;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AssignDeliveryCommandHandler(IOrderRepository orderRepository, IDeliveryStaffRepository deliveryStaffRepository, IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _orderRepository = orderRepository;
            _deliveryStaffRepository = deliveryStaffRepository;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task Handle(AssignDeliveryCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (order == null)
                throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");

            if (order.State != OrderState.ReadyForDelivery)
                throw new InvalidOperationException("Order is not in a state to be assigned.");

            var deliveryStaff = await _deliveryStaffRepository.GetDeliveryStaffByIdAsync(request.DeliveryStaffId.ToString(), cancellationToken);

            if (deliveryStaff == null || !deliveryStaff.IsAvailable)
                throw new InvalidOperationException("Delivery staff is either not available or does not exist.");

            Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId);
            await _mediator.Send(new ChangeDeliveryStaffStatusCommand
            {
                DeliveryStaffId = request.DeliveryStaffId.ToString(),
                OrderId = request.OrderId,
                IsAvailable = false
            }, cancellationToken);

            await _mediator.Send(new MoveOrderToNextPhaseCommand {
             OrderId = order.Id, StaffId = userId
            });
            await _orderRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
