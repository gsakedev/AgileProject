using MediatR;
using OrderManager.Application.Commands;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Exceptions;
using OrderManager.Domain.Factories;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Application.Handlers
{
    public class UpdateDeliveryStatusHandler : IRequestHandler<UpdateDeliveryStatusCommand>
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<DeliveryStaff> _deliveryStaffRepository;
        private readonly IDomainEventPublisher _domainEventPublisher;

        public UpdateDeliveryStatusHandler(IRepository<Order> orderRepository, IDomainEventPublisher domainEventPublisher, IRepository<DeliveryStaff> deliveryStaffRepository)
        {
            _orderRepository = orderRepository;
            _domainEventPublisher = domainEventPublisher;
            _deliveryStaffRepository = deliveryStaffRepository;
        }

        public async Task Handle(UpdateDeliveryStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            var deliverStaff = await _deliveryStaffRepository.GetByIdStringAsync(request.StaffId.ToString(), cancellationToken);

            if (deliverStaff.CurrentOrderId is null || deliverStaff.CurrentOrder.Id != order.Id)
            {
                throw new ArgumentException($"The delivery staff with ID {request.StaffId} is not assigned with the order {request.OrderId}.");
            }

            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");
            }
            order.OrderStatesB = OrderStateFactory.CreateState(order);
            order.MoveToSpecificState(request.StaffId, request.Delivered);

            await _orderRepository.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in order.DomainEvents)
            {
                await _domainEventPublisher.Publish(domainEvent, cancellationToken);
            }
        }
    }
}
