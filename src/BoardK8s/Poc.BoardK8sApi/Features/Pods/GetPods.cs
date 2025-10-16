using Carter;
using FluentResults;
using FluentValidation;
using k8s;
using Mapster;
using MediatR;
using Poc.BoardK8sApi.Entities;
using Poc.BoardK8sApi.Shared.Extensions;

namespace Poc.BoardK8sApi.Features.Pods
{
    public static class GetPods
    {
        public sealed record Query(string NamespaceName) : IRequest<Result<List<Pod>>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator() => RuleFor(r => r.NamespaceName).NotEmpty();
        }

        public sealed class Handler(KubernetesService kubernetesService, IValidator<Query> validator) : IRequestHandler<Query, Result<List<Pod>>>
        {
            private readonly KubernetesService _kubernetesService = kubernetesService;
            private readonly IValidator<Query> _validator = validator;

            public async Task<Result<List<Pod>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var validateResult = _validator.Validate(request);

                if (!validateResult.IsValid)
                    return Result.Fail(validateResult.Errors.FirstOrDefault()!.ErrorMessage);

                var pods = await _kubernetesService.ObterPods(request.NamespaceName);
                return Result.Ok(pods);
            }
        }

        public sealed class KubernetesService(IKubernetes kubeClient)
        {
            private readonly IKubernetes _kubeClient = kubeClient;

            public async Task<List<Pod>> ObterPods(string namespaceName)
            {
                var pods = await _kubeClient.CoreV1.ListNamespacedPodAsync(namespaceName);
                return pods.Adapt<List<Pod>>();
            }
        }
    }

    public class GetPodsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var endpoint = app.MapGet("api/pods/", async (string? namespaceName, ISender sender) =>
            {
                var query = new GetPods.Query(namespaceName ?? "default");
                var result = await sender.Send(query);

                return result.ToResultCustom();
            });

            ConfigMetadata(endpoint);
        }

        private void ConfigMetadata(RouteHandlerBuilder builder)
        {
            builder
                .WithTags("Pods")
                .WithDescription("Obter pods ativos no ambiente k8s.")
                .Accepts<Guid>("application/json")
                .Produces<Result<List<Pod>>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithOpenApi();
        }
    }
}
