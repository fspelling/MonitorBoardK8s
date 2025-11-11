namespace Poc.MonitorK8sPod.Infra.ExternalServices.Config
{
    public record ExternalServicesSettings
    {
        public int Timeout { get; init; } = 0;
    }
}
