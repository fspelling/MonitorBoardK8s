using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Refit;
using System.Net;

namespace Poc.MonitorK8sPod.IoC
{
    public static class RefitClientResilience
    {
        public static IServiceCollection AddRefitClientWithResilience<IInterface>(this IServiceCollection services, string baseUrl="https://default-webhook-url.com")
            where IInterface : class
        {
            services.AddRefitClient<IInterface>()
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl))
                    .AddResilienceHandler("RefitPipeline", builder =>
                    {
                        builder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
                        {
                            BackoffType = DelayBackoffType.Exponential,
                            MaxRetryAttempts = 5,
                            UseJitter = true,
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
                        });

                        builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage>()
                        {
                            BreakDuration = TimeSpan.FromMicroseconds(10),
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
                        });
                    });

            return services;
        }
    }
}
