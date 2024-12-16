using Microsoft.Extensions.Configuration;
using OrderManager.Domain;
using RabbitMQ.Client;

namespace OrderManager.Application.Services
{
    internal class RabbitMqInitializer : IRabbitMqInitializer
    {
        private readonly ConnectionFactory _factory;
        private readonly string _exchangeName;
        private readonly string _queueName;
        private readonly string _routingKey;

        public RabbitMqInitializer(IConfiguration configuration)
        {
            _factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:HostName"] ?? "rabbitmq",
                Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
                UserName = configuration["RabbitMQ:UserName"] ?? "guest",
                Password = configuration["RabbitMQ:Password"] ?? "guest"
            };

            _exchangeName = configuration["RabbitMQ:ExchangeName"] ?? "order_exchange";
            _queueName = configuration["RabbitMQ:QueueName"] ?? "my_queue";
            _routingKey = configuration["RabbitMQ:RoutingKey"] ?? "order_created";
        }
        public async Task InitializeAsync(IConfiguration configuration)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: _exchangeName,
                type: ExchangeType.Direct,
                durable: true
            );

            await channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            await channel.QueueBindAsync(
                queue: _queueName,
                exchange: _exchangeName,
                routingKey: _routingKey
            );
            Console.WriteLine("RabbitMQ exchanges, queues, and bindings initialized successfully.");
        }
    }
}
