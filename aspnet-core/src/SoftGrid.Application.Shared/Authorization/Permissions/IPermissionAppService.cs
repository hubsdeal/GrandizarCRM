using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization.Permissions.Dto;

namespace SoftGrid.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
