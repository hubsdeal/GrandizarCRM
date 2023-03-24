using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreBusinessHoursAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreBusinessHourForViewDto>> GetAll(GetAllStoreBusinessHoursInput input);

        Task<GetStoreBusinessHourForViewDto> GetStoreBusinessHourForView(long id);

        Task<GetStoreBusinessHourForEditOutput> GetStoreBusinessHourForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreBusinessHourDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreBusinessHoursToExcel(GetAllStoreBusinessHoursForExcelInput input);

        Task<PagedResultDto<StoreBusinessHourStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreBusinessHourMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreBusinessHourMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input);

    }
}