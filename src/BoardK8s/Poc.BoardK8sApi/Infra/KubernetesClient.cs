using k8s;

namespace Poc.BoardK8sApi.Infra
{
    public class KubernetesClient
    {
        private readonly IKubernetes _client;

        public KubernetesClient()
        {
            var mode = Environment.GetEnvironmentVariable("K8S_MODE")!;

            if (mode == "K8S")
                _client = new Kubernetes(KubernetesClientConfiguration.InClusterConfig());
            else
                _client = new Kubernetes(KubernetesClientConfiguration.BuildConfigFromConfigFile());
        }

        public IKubernetes Client => _client;
    }
}