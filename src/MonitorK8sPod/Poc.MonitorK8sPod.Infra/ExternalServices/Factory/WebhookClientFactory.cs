using Poc.MonitorK8sPod.Infra.ExternalServices.Apis.Interfaces;
using Refit;

namespace Poc.MonitorK8sPod.Infra.ExternalServices.Factory
{
    public class WebhookClientFactory
    {
        public IWebhookAPI CreateClient(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("A URL base não pode ser nula ou vazia.", nameof(baseUrl));

            return RestService.For<IWebhookAPI>(baseUrl);
        }
    }
}
