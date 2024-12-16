using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace OrderManager.Application.Services
{
    public class RabbitMqService
    {
        private readonly IConfiguration _configuration;

        public RabbitMqService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public  async Task<IConnection> GetConnectionAsync()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQ:HostName"],  // Use "rabbitmq" as the hostname
                Port = int.Parse(_configuration["RabbitMQ:Port"]),
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"]
            };

            return await factory.CreateConnectionAsync();
        }
    }
}
