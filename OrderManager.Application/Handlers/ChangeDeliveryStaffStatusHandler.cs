using MediatR;
using OrderManager.Application.Commands;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Application.Handlers
{
    public class ChangeDeliveryStaffStatusHandler : IRequestHandler<ChangeDeliveryStaffStatusCommand>
    {
        private readonly IDeliveryStaffRepository _deliveryStaffRepository;

        public ChangeDeliveryStaffStatusHandler(IDeliveryStaffRepository deliveryStaffRepository)
        {
            _deliveryStaffRepository = deliveryStaffRepository;
        }

        public async Task Handle(ChangeDeliveryStaffStatusCommand request, CancellationToken cancellationToken)
        {
            var deliveryStaff = await _deliveryStaffRepository.GetDeliveryStaffByIdAsync(request.DeliveryStaffId, cancellationToken);

            if (deliveryStaff == null)
            {
                throw new KeyNotFoundException($"Delivery staff with ID {request.DeliveryStaffId} not found.");
            }

            if (request.IsAvailable)
            {
                deliveryStaff.IsAvailable = true;
                deliveryStaff.CurrentOrderId = null;
            }
            else
            {
                if (request.OrderId == null)
                {
                    throw new InvalidOperationException("OrderId must be provided when setting delivery staff unavailable.");
                }

                deliveryStaff.IsAvailable = false;
                deliveryStaff.CurrentOrderId = request.OrderId;
            }

            await _deliveryStaffRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
