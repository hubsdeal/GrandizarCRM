using System.Threading.Tasks;
using Abp.Application.Services;
using SoftGrid.Configuration.Tenants.Dto;

namespace SoftGrid.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
