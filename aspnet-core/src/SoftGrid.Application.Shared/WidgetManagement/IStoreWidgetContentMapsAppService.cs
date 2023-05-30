using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.WidgetManagement
{
    public interface IStoreWidgetContentMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreWidgetContentMapForViewDto>> GetAll(GetAllStoreWidgetContentMapsInput input);

        Task<GetStoreWidgetContentMapForViewDto> GetStoreWidgetContentMapForView(long id);

        Task<GetStoreWidgetContentMapForEditOutput> GetStoreWidgetContentMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreWidgetContentMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreWidgetContentMapsToExcel(GetAllStoreWidgetContentMapsForExcelInput input);

        Task<PagedResultDto<StoreWidgetContentMapStoreWidgetMapLookupTableDto>> GetAllStoreWidgetMapForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreWidgetContentMapContentLookupTableDto>> GetAllContentForLookupTable(GetAllForLookupTableInput input);

    }
}