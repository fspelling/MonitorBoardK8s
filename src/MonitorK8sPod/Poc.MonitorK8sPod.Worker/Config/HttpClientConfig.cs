using Poc.MonitorK8sPod.Infra.ExternalServices.Apis.Interfaces;
using Poc.MonitorK8sPod.IoC;

namespace Poc.MonitorK8sPod.Worker.Config
{
    public static class HttpClientConfig
    {
        public static IServiceCollection RegisterHttpClient(this IServiceCollection services)
        {
            services.AddRefitClientWithResilience<IWebhookAPI>();
            return services;
        }
    }
}
