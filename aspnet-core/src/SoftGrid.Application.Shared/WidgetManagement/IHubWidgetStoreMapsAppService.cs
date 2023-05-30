using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.WidgetManagement
{
    public interface IHubWidgetStoreMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubWidgetStoreMapForViewDto>> GetAll(GetAllHubWidgetStoreMapsInput input);

        Task<GetHubWidgetStoreMapForViewDto> GetHubWidgetStoreMapForView(long id);

        Task<GetHubWidgetStoreMapForEditOutput> GetHubWidgetStoreMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubWidgetStoreMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubWidgetStoreMapsToExcel(GetAllHubWidgetStoreMapsForExcelInput input);

        Task<PagedResultDto<HubWidgetStoreMapHubWidgetMapLookupTableDto>> GetAllHubWidgetMapForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubWidgetStoreMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}