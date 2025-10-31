using Poc.MonitorK8sPod.Domain.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Poc.MonitorK8sPod.Infra.Messaging
{
    public sealed class RabbitmqProducer(IConnectionFactory connectionFactory) : IMessagingProducer, IAsyncDisposable
    {
        private readonly IConnectionFactory _connectionFactory = connectionFactory;

        private IConnection? _connection;
        private IChannel? _channel;

        public async Task PublishAsync<T>(T message, string exchangeName, string routingKey, string queueName)
        {
            if (_connection is null || _channel is null)
            {
                _connection = await _connectionFactory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
            }

            await _channel!.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct);
            await _channel!.QueueDeclareAsync(queueName, true, false, false);
            await _channel!.QueueBindAsync(queueName, exchangeName, routingKey);

            await _channel!.BasicPublishAsync(
                exchangeName,
                routingKey, 
                false, 
                new BasicProperties(),
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message))
            );
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel is not null)
                await _channel.DisposeAsync();

            if (_connection is not null)
                await _connection.DisposeAsync();
        }
    }
}
