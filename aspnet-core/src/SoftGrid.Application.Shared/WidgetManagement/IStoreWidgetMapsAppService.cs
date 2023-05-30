using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.WidgetManagement
{
    public interface IStoreWidgetMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreWidgetMapForViewDto>> GetAll(GetAllStoreWidgetMapsInput input);

        Task<GetStoreWidgetMapForViewDto> GetStoreWidgetMapForView(long id);

        Task<GetStoreWidgetMapForEditOutput> GetStoreWidgetMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreWidgetMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreWidgetMapsToExcel(GetAllStoreWidgetMapsForExcelInput input);

        Task<PagedResultDto<StoreWidgetMapMasterWidgetLookupTableDto>> GetAllMasterWidgetForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreWidgetMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}