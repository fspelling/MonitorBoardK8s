using MediatR;
using Poc.MonitorK8sPod.Domain.Entities;
using Poc.MonitorK8sPod.Domain.Events;
using Poc.MonitorK8sPod.Infra.ExternalServices.Factory;
using Poc.MonitorK8sPod.Shared.Results;

namespace Poc.MonitorK8sPod.Application.Commands
{
    public sealed class WebhookCommandHandler(WebhookClientFactory webhookClientFactory) : IRequestHandler<WebhookCommand, Result>
    {
        private readonly WebhookClientFactory _webhookClientFactory = webhookClientFactory;

        private HashSet<WebhookEndpoints> _webhooks =
        [
            new WebhookEndpoints("https://localhost:7171/api/webhook/pods", nameof(PodCreatedEvent), true)
        ];

        public async Task<Result> Handle(WebhookCommand request, CancellationToken cancellationToken)
        {
            if (request.PodData is null)
                return Result.Failure("Erro ao buscar PodData.");

            foreach (var webhook in _webhooks)
            {
                var webhookClient = _webhookClientFactory.CreateClient(webhook.Endpoint);

                await webhookClient.NotifyAsync(
                    new { 
                        EventName = nameof(PodCreatedEvent),
                        Data = request.PodData 
                    }
                );
            }

            return Result.Success();
        }
    }
}
