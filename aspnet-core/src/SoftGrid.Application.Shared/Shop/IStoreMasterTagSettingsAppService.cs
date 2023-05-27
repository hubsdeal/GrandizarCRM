using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreMasterTagSettingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreMasterTagSettingForViewDto>> GetAll(GetAllStoreMasterTagSettingsInput input);

        Task<GetStoreMasterTagSettingForViewDto> GetStoreMasterTagSettingForView(long id);

        Task<GetStoreMasterTagSettingForEditOutput> GetStoreMasterTagSettingForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreMasterTagSettingDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreMasterTagSettingsToExcel(GetAllStoreMasterTagSettingsForExcelInput input);

        Task<PagedResultDto<StoreMasterTagSettingStoreTagSettingCategoryLookupTableDto>> GetAllStoreTagSettingCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreMasterTagSettingMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

    }
}