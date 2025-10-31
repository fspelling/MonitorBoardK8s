using MediatR;
using Poc.MonitorK8sPod.Domain.Events;
using Poc.MonitorK8sPod.Domain.Interfaces;

namespace Poc.MonitorK8sPod.Worker
{
    public class Worker(IMediator mediator, IPodWatcher podWatcher) : BackgroundService
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPodWatcher _podWatcher = podWatcher;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _podWatcher.PodCreated += async (sender, pod) =>
                await _mediator.Publish(new PodCreatedEvent(pod), stoppingToken);

            await _podWatcher.StartWatchingAsync(stoppingToken);
        }
    }
}
