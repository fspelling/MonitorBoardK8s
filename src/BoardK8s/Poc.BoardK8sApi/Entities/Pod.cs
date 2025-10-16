namespace Poc.BoardK8sApi.Entities
{
    public record Pod(Guid Uid, string Name, string NamespaceProperty, List<Container> Containers);
}
