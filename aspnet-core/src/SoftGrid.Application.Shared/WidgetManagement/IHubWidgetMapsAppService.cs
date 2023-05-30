using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.WidgetManagement
{
    public interface IHubWidgetMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubWidgetMapForViewDto>> GetAll(GetAllHubWidgetMapsInput input);

        Task<GetHubWidgetMapForViewDto> GetHubWidgetMapForView(long id);

        Task<GetHubWidgetMapForEditOutput> GetHubWidgetMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubWidgetMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubWidgetMapsToExcel(GetAllHubWidgetMapsForExcelInput input);

        Task<PagedResultDto<HubWidgetMapHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubWidgetMapMasterWidgetLookupTableDto>> GetAllMasterWidgetForLookupTable(GetAllForLookupTableInput input);

    }
}