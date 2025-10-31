using k8s;
using Mapster;
using Poc.MonitorK8sPod.Domain.Entities;
using Poc.MonitorK8sPod.Domain.Interfaces;

namespace Poc.MonitorK8sPod.Infra.Kubernetes
{
    public sealed class K8sPodWatcher(IKubernetes kubernetesClient) : IPodWatcher
    {
        private readonly IKubernetes _kubernetesClient = kubernetesClient;

        public event EventHandler<Pod>? PodCreated;

        public async Task StartWatchingAsync(CancellationToken cancellationToken)
        {
            using var watcher = _kubernetesClient.CoreV1.WatchListNamespacedPod(
                namespaceParameter: "default",

                onEvent: (type, pod) =>
                {
                    if (type == WatchEventType.Added)
                    {
                        var domainPod = pod.Adapt<Pod>();
                        PodCreated?.Invoke(this, domainPod);
                    }

                    Console.WriteLine($"Evento: {type}, Pod: {pod.Metadata.Name}");
                },

                onError: e => Console.WriteLine($"Erro: {e}."),
                onClosed: () => Console.WriteLine("Watcher encerrado, reiniciando...")
            );

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
    }
}
