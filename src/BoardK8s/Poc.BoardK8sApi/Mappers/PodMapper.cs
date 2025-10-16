using k8s.Models;
using Mapster;
using Poc.BoardK8sApi.Entities;

namespace Poc.BoardK8sApi.Mappers
{
    public static class PodMapper
    {
        public static void Register()
        {
            TypeAdapterConfig<V1Pod, Pod>.NewConfig()
                .Map(dest => dest.Uid, src => src.Metadata!.Uid)
                .Map(dest => dest.Name, src => src.Metadata!.Name)
                .Map(dest => dest.Containers, src => src.Spec!.Containers.Select(c => new Container(c.Name, c.Image )).ToList());
        }
    }
}
