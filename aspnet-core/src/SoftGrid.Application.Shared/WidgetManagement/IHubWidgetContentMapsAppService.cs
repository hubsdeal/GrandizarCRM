using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.WidgetManagement
{
    public interface IHubWidgetContentMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubWidgetContentMapForViewDto>> GetAll(GetAllHubWidgetContentMapsInput input);

        Task<GetHubWidgetContentMapForViewDto> GetHubWidgetContentMapForView(long id);

        Task<GetHubWidgetContentMapForEditOutput> GetHubWidgetContentMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubWidgetContentMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubWidgetContentMapsToExcel(GetAllHubWidgetContentMapsForExcelInput input);

        Task<PagedResultDto<HubWidgetContentMapHubWidgetMapLookupTableDto>> GetAllHubWidgetMapForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubWidgetContentMapContentLookupTableDto>> GetAllContentForLookupTable(GetAllForLookupTableInput input);

    }
}