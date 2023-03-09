using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace SoftGrid.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
