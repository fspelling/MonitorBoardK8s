using Refit;

namespace Poc.MonitorK8sPod.Infra.ExternalServices.Apis.Interfaces
{
    [Headers("accept: */*")]
    public interface IWebhookAPI
    {
        [Post("")]
        Task NotifyAsync([Body] object payload);
    }
}
