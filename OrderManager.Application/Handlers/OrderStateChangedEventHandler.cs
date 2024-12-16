using MediatR;
using OrderManager.Application.Notifications;
using OrderManager.Domain.Entities;
using OrderManager.Domain.Interfaces;

namespace OrderManager.Application.Handlers
{
    public class OrderStateChangedEventHandler : INotificationHandler<OrderStateChangedNotification>
    {
        private readonly IRepository<OrderStateAudit> _auditRepository;

        public OrderStateChangedEventHandler(IRepository<OrderStateAudit> auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task Handle(OrderStateChangedNotification notification, CancellationToken cancellationToken)
        {
            var audit = new OrderStateAudit
            {
                OrderId = notification.OrderId,
                FromState = notification.FromState,
                ToState = notification.ToState,
                StaffId = notification.StaffId,
                Timestamp = notification.Timestamp
            };

            await _auditRepository.AddAsync(audit, cancellationToken);
            await _auditRepository.SaveChangesAsync(cancellationToken);

        }
    }
}
