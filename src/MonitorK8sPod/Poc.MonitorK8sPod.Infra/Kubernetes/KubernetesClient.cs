using k8s;

namespace Poc.MonitorK8sPod.Infra.Kubernetes
{
    public class KubernetesClient
    {
        private readonly IKubernetes _client;

        public KubernetesClient()
        {
            var mode = Environment.GetEnvironmentVariable("K8S_MODE")!;

            if (mode == "K8S")
                _client = new k8s.Kubernetes(KubernetesClientConfiguration.InClusterConfig());
            else
                _client = new k8s.Kubernetes(KubernetesClientConfiguration.BuildConfigFromConfigFile());
        }

        public IKubernetes Client => _client;
    }
}