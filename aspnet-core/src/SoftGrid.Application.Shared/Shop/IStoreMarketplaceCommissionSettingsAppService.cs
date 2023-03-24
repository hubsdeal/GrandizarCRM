using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreMarketplaceCommissionSettingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreMarketplaceCommissionSettingForViewDto>> GetAll(GetAllStoreMarketplaceCommissionSettingsInput input);

        Task<GetStoreMarketplaceCommissionSettingForViewDto> GetStoreMarketplaceCommissionSettingForView(long id);

        Task<GetStoreMarketplaceCommissionSettingForEditOutput> GetStoreMarketplaceCommissionSettingForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreMarketplaceCommissionSettingDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreMarketplaceCommissionSettingsToExcel(GetAllStoreMarketplaceCommissionSettingsForExcelInput input);

        Task<PagedResultDto<StoreMarketplaceCommissionSettingStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableDto>> GetAllMarketplaceCommissionTypeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreMarketplaceCommissionSettingProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreMarketplaceCommissionSettingProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

    }
}