using MediatR;
using OrderManager.Application.Commands;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Factories;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Application.Handlers
{
    public class CancelOrderHandler : IRequestHandler<CancelOrderCommand>
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IDomainEventPublisher _domainEventPublisher;

        public CancelOrderHandler(IRepository<Order> orderRepository, IDomainEventPublisher domainEventPublisher)
        {
            _orderRepository = orderRepository;
            _domainEventPublisher = domainEventPublisher;
        }

        public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");
            }
            order.OrderStatesB = OrderStateFactory.CreateState(order);

            order.CancelOrder(request.StaffId);

            await _orderRepository.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in order.DomainEvents)
            {
                await _domainEventPublisher.Publish(domainEvent, cancellationToken);
            }

            order.ClearDomainEvents();
        }
    }
}
