namespace Poc.MonitorK8sPod.Domain.Interfaces
{
    public interface IMessagingProducer
    {
        Task PublishAsync<T>(T message, string exchangeName, string routingKey, string queueName);
    }
}
