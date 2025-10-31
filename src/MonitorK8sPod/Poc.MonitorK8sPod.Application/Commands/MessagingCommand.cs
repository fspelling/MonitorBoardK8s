using MediatR;
using Poc.MonitorK8sPod.Shared.Results;

namespace Poc.MonitorK8sPod.Application.Commands
{
    public record MessagingCommand(string PodData) : IRequest<Result>;
}
