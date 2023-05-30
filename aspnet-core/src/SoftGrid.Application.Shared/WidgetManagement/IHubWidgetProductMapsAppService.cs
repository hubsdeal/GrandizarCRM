using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.WidgetManagement
{
    public interface IHubWidgetProductMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubWidgetProductMapForViewDto>> GetAll(GetAllHubWidgetProductMapsInput input);

        Task<GetHubWidgetProductMapForViewDto> GetHubWidgetProductMapForView(long id);

        Task<GetHubWidgetProductMapForEditOutput> GetHubWidgetProductMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubWidgetProductMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubWidgetProductMapsToExcel(GetAllHubWidgetProductMapsForExcelInput input);

        Task<PagedResultDto<HubWidgetProductMapHubWidgetMapLookupTableDto>> GetAllHubWidgetMapForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubWidgetProductMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

    }
}