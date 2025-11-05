using Poc.MonitorK8sPod.Infra.ExternalServices.Apis.Interfaces;
using Refit;

namespace Poc.MonitorK8sPod.Infra.ExternalServices.Factory
{
    public class WebhookClientFactory(IHttpClientFactory httpClientFactory)
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public IWebhookAPI CreateClient(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("A URL base não pode ser nula ou vazia.", nameof(baseUrl));

            var httpClient = _httpClientFactory.CreateClient(nameof(IWebhookAPI));
            httpClient.BaseAddress = new Uri(baseUrl);

            return RestService.For<IWebhookAPI>(baseUrl);
        }
    }
}
