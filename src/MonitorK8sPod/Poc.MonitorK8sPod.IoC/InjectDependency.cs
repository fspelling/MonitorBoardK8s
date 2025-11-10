using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Poc.MonitorK8sPod.Domain.Interfaces;
using Poc.MonitorK8sPod.Infra.ExternalServices.Factory;
using Poc.MonitorK8sPod.Infra.Kubernetes;
using Poc.MonitorK8sPod.Infra.Messaging;
using Poc.MonitorK8sPod.Infra.Messaging.Config;
using RabbitMQ.Client;

namespace Poc.MonitorK8sPod.IoC
{
    public static class InjectDependency
    {
        public static void Register(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitmqSettings>(configuration.GetSection(nameof(RabbitmqSettings)));

            services.AddSingleton<IConnectionFactory>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<RabbitmqSettings>>().Value;
                return new ConnectionFactory()
                {
                    HostName = settings.HostName,
                    UserName = settings.UserName,
                    Password = settings.Password,
                    Port = settings.Port
                };
            });

            services.AddSingleton<IMessagingProducer, RabbitmqProducer>();
            services.AddSingleton<KubernetesClient>();
            services.AddSingleton(sp => sp.GetRequiredService<KubernetesClient>().Client);
            services.AddSingleton<WebhookClientFactory>();
            services.AddSingleton<IPodWatcher, K8sPodWatcher>();
        }
    }
}
