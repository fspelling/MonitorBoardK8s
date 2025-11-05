using Poc.MonitorK8sPod.IoC;

namespace Poc.MonitorK8sPod.Worker.Config
{
    public static class MapperConfig
    {
        public static IServiceCollection RegisterMapperObjects(this IServiceCollection services)
        {
            MapperObjects.Register();
            return services;
        }
    }
}
