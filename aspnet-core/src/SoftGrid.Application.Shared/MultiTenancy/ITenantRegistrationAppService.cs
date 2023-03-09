using System.Threading.Tasks;
using Abp.Application.Services;
using SoftGrid.Editions.Dto;
using SoftGrid.MultiTenancy.Dto;

namespace SoftGrid.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}