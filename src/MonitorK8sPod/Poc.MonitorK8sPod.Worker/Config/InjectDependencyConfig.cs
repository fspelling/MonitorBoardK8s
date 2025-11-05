using Poc.MonitorK8sPod.IoC;

namespace Poc.MonitorK8sPod.Worker.Config
{
    public static class InjectDependencyConfig
    {
        public static IServiceCollection RegisterDI(this IServiceCollection services, IConfiguration configuration)
        {
            InjectDependency.Register(services, configuration);
            return services;
        }
    }
}
