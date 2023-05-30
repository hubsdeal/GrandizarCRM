using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.WidgetManagement
{
    public interface IStoreThemeMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreThemeMapForViewDto>> GetAll(GetAllStoreThemeMapsInput input);

        Task<GetStoreThemeMapForViewDto> GetStoreThemeMapForView(long id);

        Task<GetStoreThemeMapForEditOutput> GetStoreThemeMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreThemeMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreThemeMapsToExcel(GetAllStoreThemeMapsForExcelInput input);

        Task<PagedResultDto<StoreThemeMapStoreMasterThemeLookupTableDto>> GetAllStoreMasterThemeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreThemeMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}