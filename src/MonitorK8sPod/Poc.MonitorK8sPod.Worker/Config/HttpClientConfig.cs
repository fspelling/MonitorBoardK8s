using Poc.MonitorK8sPod.Infra.ExternalServices.Config;
using Poc.MonitorK8sPod.IoC;

namespace Poc.MonitorK8sPod.Worker.Config
{
    public static class HttpClientConfig
    {
        public static IServiceCollection RegisterHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            var externalServicesSettings = configuration.GetSection("ExternalServicesSettings").Get<ExternalServicesSettings>()!;

            services.AddHttpClientWithResilience("Webhook", externalServicesSettings.Timeout);
            return services;
        }
    }
}
