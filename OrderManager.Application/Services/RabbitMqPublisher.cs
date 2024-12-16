using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using OrderManager.Domain.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace OrderManager.Application.Services
{
    public class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
    {
        private readonly ConnectionFactory _factory;
        private  IConnection _connection;
        private  IChannel _channel;

        public  RabbitMqPublisher(string hostName, int port, string userName, string password)
        {
            _factory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password
            };

        }

        private async Task InitializeAsync()
        {
            if (_connection == null)
            {
                _connection = await _factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
            }
        }
        public async Task Publish<T>(string exchange, string routingKey, T message)
        {
            await InitializeAsync();

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            await _channel.BasicPublishAsync(exchange, routingKey, body);
            Console.WriteLine($"[x] Sent message to exchange '{exchange}' with routing key '{routingKey}': {json}");

        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null)
                await _channel.CloseAsync();

            if (_connection != null)
                await _connection.CloseAsync();

            Console.WriteLine("RabbitMQ connection closed.");
        }
    }
}
