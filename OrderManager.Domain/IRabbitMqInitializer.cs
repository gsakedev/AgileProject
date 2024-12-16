using Microsoft.Extensions.Configuration;

namespace OrderManager.Domain
{
    public interface IRabbitMqInitializer
    {
        Task InitializeAsync(IConfiguration configuration);
    }
}
