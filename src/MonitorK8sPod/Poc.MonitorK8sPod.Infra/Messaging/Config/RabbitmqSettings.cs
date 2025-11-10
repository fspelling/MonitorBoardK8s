namespace Poc.MonitorK8sPod.Infra.Messaging.Config
{
    public record RabbitmqSettings
    {
        public string HostName { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string Port { get; init; } = string.Empty;
    }
}
