using System.Threading.Tasks;
using Abp.Webhooks;

namespace SoftGrid.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
