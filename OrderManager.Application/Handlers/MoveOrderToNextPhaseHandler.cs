using MediatR;
using OrderManager.Application.Commands;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Factories;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Application.Handlers
{
    public class MoveOrderToNextPhaseHandler : IRequestHandler<MoveOrderToNextPhaseCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDomainEventPublisher _domainEventPublisher;
        private readonly IMessagePublisher _publisher;

        public MoveOrderToNextPhaseHandler(IOrderRepository orderRepository, IDomainEventPublisher domainEventPublisher, IMessagePublisher publisher)
        {
            _publisher = publisher;

            _orderRepository = orderRepository;
            _domainEventPublisher = domainEventPublisher;
        }
         public async Task Handle(MoveOrderToNextPhaseCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");
            }
            order.OrderStatesB = OrderStateFactory.CreateState(order);
            order.MoveToNextState(request.StaffId);

            await _orderRepository.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in order.DomainEvents)
            {
                await _domainEventPublisher.Publish(domainEvent, cancellationToken);
            }
            order.ClearDomainEvents();
            //TODO: for autoassignment
            //await _publisher.Publish("order_exchange", "order_created", request.OrderId);
        }
    }
}


