using System.Threading.Tasks;
using Abp.Application.Services;
using SoftGrid.Sessions.Dto;

namespace SoftGrid.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
