namespace Poc.MonitorK8sPod.Domain.Entities
{
    public record WebhookEndpoints(string Endpoint, string Event, bool Ativo);
}
