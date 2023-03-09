using System.Threading.Tasks;
using Abp.Application.Services;

namespace SoftGrid.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
