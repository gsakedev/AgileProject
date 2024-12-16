namespace OrderManager.Domain.Interfaces
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
