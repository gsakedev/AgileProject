using MediatR;
using OrderManager.Application.Commands;
using OrderManager.Application.Mappers;
using OrderManager.Application.Notifications;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Events;
using OrderManager.Domain.Interfaces;
namespace OrderManager.Application.Handlers
{
    public class DomainEventPublisher : IDomainEventPublisher
    {
        private readonly IMediator _mediator;
        public DomainEventPublisher(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Publish(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            if (domainEvent is OrderStateChangedEvent orderStateChangedEvent)
            {
                var notification = DomainEventMapper.ToNotification(orderStateChangedEvent);
                await _mediator.Publish(notification, cancellationToken);
            }
            else if (domainEvent is DeliveryStaffStatusChangeEvent deliveryStaffStatusChangeEvent)
            {


                await _mediator.Send(new ChangeDeliveryStaffStatusCommand
                {
                    OrderId = deliveryStaffStatusChangeEvent.OrderId,
                    DeliveryStaffId = deliveryStaffStatusChangeEvent.DeliveryStaffId,
                    IsAvailable = deliveryStaffStatusChangeEvent.IsAvailable
                }, cancellationToken);
            }
        }
    }
}
