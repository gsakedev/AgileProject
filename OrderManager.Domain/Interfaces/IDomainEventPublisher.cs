namespace OrderManager.Domain.Interfaces
{
    public interface IDomainEventPublisher
    {
        Task Publish(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }
}
