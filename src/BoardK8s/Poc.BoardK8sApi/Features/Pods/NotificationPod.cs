using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Poc.BoardK8sApi.Entities;
using Poc.BoardK8sApi.Shared.Results;

namespace Poc.BoardK8sApi.Features.Pods
{
    public static class NotificationPod
    {
        public record InputModel(string Data);
        public sealed record Command(string Data) : IRequest<Result>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator() => RuleFor(r => r.Data).NotEmpty();
        }

        public sealed class Handler(IValidator<Command> validator) : IRequestHandler<Command, Result>
        {
            private readonly IValidator<Command> _validator = validator;

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var validateResult = await _validator.ValidateAsync(request);

                if (!validateResult.IsValid)
                    return Result<List<Pod>?>.Failure(validateResult.Errors.FirstOrDefault()!.ErrorMessage);

                return Result.Success();
            }
        }

        public interface INotificationPodClient
        {
            Task ReceiveNotificationPod(string requestWebhook);
        }

        public sealed class NotificationPodHub : Hub<INotificationPodClient>
        {
        }

        public class NotificationPodEndpoint : ICarterModule
        {
            public void AddRoutes(IEndpointRouteBuilder app)
            {
                var endpoint = app.MapPost("api/pods/notification", async ([FromBody] InputModel request, ISender sender, IHubContext<NotificationPodHub, INotificationPodClient> context) =>
                {
                    var command = request.Adapt<Command>();
                    var result = await sender.Send(command);

                    if (!result.IsSuccess)
                        Results.BadRequest(result.MsgError);

                    await context.Clients.All.ReceiveNotificationPod(request.Data);
                    return Results.Ok(result);
                });

                ConfigMetadata(endpoint);
            }

            private void ConfigMetadata(RouteHandlerBuilder builder)
            {
                builder
                    .WithTags("Pods")
                    .WithDescription("Notificar pods que forao criados no ambiente k8s.")
                    .Accepts<Guid>("application/json")
                    .Produces<Result>(StatusCodes.Status200OK)
                    .ProducesProblem(StatusCodes.Status400BadRequest)
                    .WithOpenApi();
            }
        }
    }
}
