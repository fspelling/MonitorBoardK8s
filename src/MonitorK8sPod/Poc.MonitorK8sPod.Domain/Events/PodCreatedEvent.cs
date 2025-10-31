using MediatR;
using Poc.MonitorK8sPod.Domain.Entities;

namespace Poc.MonitorK8sPod.Domain.Events
{
    public record PodCreatedEvent(Pod Pod) : INotification
    {
        public DateTime DateCreated { get; } = DateTime.Now;
    }
}
