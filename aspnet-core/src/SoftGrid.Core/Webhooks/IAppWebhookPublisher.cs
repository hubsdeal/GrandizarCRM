using System.Threading.Tasks;
using SoftGrid.Authorization.Users;

namespace SoftGrid.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
