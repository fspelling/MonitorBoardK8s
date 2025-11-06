using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Refit;
using System.Net;

namespace Poc.MonitorK8sPod.IoC
{
    public static class ExternalServiceResilience
    {
        public static IServiceCollection AddHttpClientWithResilience(this IServiceCollection services, string httpClientName)
        {
            services.AddHttpClient(httpClientName)
                    .AddResilienceHandler("HttpClientPipeline", builder =>
                    {
                        builder.AddRetry(SetConfigRetry());
                        builder.AddCircuitBreaker(SetConfigCircuitBreaker());
                    });

            return services;
        }

        public static IServiceCollection AddRefitClientWithResilience<IInterface>(this IServiceCollection services, string baseUrl)
            where IInterface : class
        {
            services.AddRefitClient<IInterface>()
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl))
                    .AddResilienceHandler("RefitPipeline", builder =>
                    {
                        builder.AddRetry(SetConfigRetry());
                        builder.AddCircuitBreaker(SetConfigCircuitBreaker());
                    });

            return services;
        }

        private static RetryStrategyOptions<HttpResponseMessage> SetConfigRetry()
        {
            return new RetryStrategyOptions<HttpResponseMessage>
            {
                BackoffType = DelayBackoffType.Exponential,
                MaxRetryAttempts = 5,
                UseJitter = true,
                OnRetry = static args =>
                {
                    return new ValueTask(Task.Run(() => Console.WriteLine($"[Webhook Retry] Tentativa #{args.AttemptNumber} - Erro: {args.Outcome.Exception?.Message}")));
                },
                ShouldHandle = static args =>
                {
                    return ValueTask.FromResult(args.Outcome switch
                    {
                        { Result.StatusCode: HttpStatusCode.TooManyRequests } => true,
                        { Result.StatusCode: HttpStatusCode.RequestTimeout } => true,
                        { Result.StatusCode: HttpStatusCode.InternalServerError } => true,
                        { Exception: not null } => true,
                        _ => false
                    });
                }
            };
        }

        private static CircuitBreakerStrategyOptions<HttpResponseMessage> SetConfigCircuitBreaker()
        {
            return new CircuitBreakerStrategyOptions<HttpResponseMessage>()
            {
                BreakDuration = TimeSpan.FromSeconds(10),
                MinimumThroughput = 5,
                FailureRatio = 0.2,
                ShouldHandle = static args =>
                {
                    return ValueTask.FromResult(args.Outcome switch
                    {
                        { Result.StatusCode: HttpStatusCode.TooManyRequests } => true,
                        { Result.StatusCode: HttpStatusCode.RequestTimeout } => true,
                        { Result.StatusCode: HttpStatusCode.InternalServerError } => true,
                        _ => false
                    });
                }
            };
        }
    }
}
