namespace OrderManager.Domain.Interfaces
{
    public interface IMessagePublisher
    {
        Task Publish<T>(string exchange, string routingKey, T message);
    }
}
