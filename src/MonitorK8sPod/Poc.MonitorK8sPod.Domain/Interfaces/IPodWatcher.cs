using Poc.MonitorK8sPod.Domain.Entities;

namespace Poc.MonitorK8sPod.Domain.Interfaces
{
    public interface IPodWatcher
    {
        event EventHandler<Pod>? PodCreated;
        Task StartWatchingAsync(CancellationToken cancellationToken);
    }
}
