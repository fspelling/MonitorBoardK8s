using MediatR;
using Poc.MonitorK8sPod.Domain.Interfaces;
using Poc.MonitorK8sPod.Shared.Results;

namespace Poc.MonitorK8sPod.Application.Commands
{
    public sealed class MessagingCommandHandler(IMessagingProducer messagingProducer) : IRequestHandler<MessagingCommand, Result>
    {
        private readonly IMessagingProducer _messagingProducer = messagingProducer;

        public async Task<Result> Handle(MessagingCommand request, CancellationToken cancellationToken)
        {
            if (request.PodData is null)
                return Result.Failure("Erro ao buscar message.");

            await _messagingProducer.PublishAsync(request.PodData, "exchange-pod", "pod-create", "pod-create");
            return Result.Success();
        }
    }
}
