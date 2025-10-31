using Poc.MonitorK8sPod.IoC.MapAdapt;

namespace Poc.MonitorK8sPod.Worker.Config
{
    public static class MapperAddaptConfig
    {
        public static void Register()
        {
            PodMapper.Register();
        }
    }
}
