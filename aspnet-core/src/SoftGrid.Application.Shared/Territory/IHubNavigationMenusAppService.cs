using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Territory
{
    public interface IHubNavigationMenusAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubNavigationMenuForViewDto>> GetAll(GetAllHubNavigationMenusInput input);

        Task<GetHubNavigationMenuForViewDto> GetHubNavigationMenuForView(long id);

        Task<GetHubNavigationMenuForEditOutput> GetHubNavigationMenuForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubNavigationMenuDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubNavigationMenusToExcel(GetAllHubNavigationMenusForExcelInput input);

        Task<PagedResultDto<HubNavigationMenuHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubNavigationMenuMasterNavigationMenuLookupTableDto>> GetAllMasterNavigationMenuForLookupTable(GetAllForLookupTableInput input);

    }
}