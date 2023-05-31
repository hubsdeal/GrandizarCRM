using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.WidgetManagement
{
    public interface IStoreWidgetProductMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreWidgetProductMapForViewDto>> GetAll(GetAllStoreWidgetProductMapsInput input);

        Task<GetStoreWidgetProductMapForViewDto> GetStoreWidgetProductMapForView(long id);

        Task<GetStoreWidgetProductMapForEditOutput> GetStoreWidgetProductMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreWidgetProductMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreWidgetProductMapsToExcel(GetAllStoreWidgetProductMapsForExcelInput input);

        Task<PagedResultDto<StoreWidgetProductMapStoreWidgetMapLookupTableDto>> GetAllStoreWidgetMapForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreWidgetProductMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

    }
}