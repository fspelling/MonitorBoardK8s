using MediatR;
using Poc.MonitorK8sPod.Application.Commands;
using Poc.MonitorK8sPod.Domain.Events;
using System.Text.Json;

namespace Poc.MonitorK8sPod.Application.Events.Handlers
{
    public sealed class WebhookPodEventHandler(IMediator mediator) : INotificationHandler<PodCreatedEvent>
    {
        private readonly IMediator _mediator = mediator;

        public async Task Handle(PodCreatedEvent notification, CancellationToken cancellationToken)
        {
            var command = new WebhookCommand(JsonSerializer.Serialize(notification.Pod));
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                Console.WriteLine($"Erro ao processar requisicao do webhook: {result.MsgError}");
        }
    }
}
