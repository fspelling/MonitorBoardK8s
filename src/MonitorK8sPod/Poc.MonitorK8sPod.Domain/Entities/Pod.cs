namespace Poc.MonitorK8sPod.Domain.Entities
{
    public record Pod(Guid Uid, string Name, string NamespaceProperty, List<Container> Containers);
}
