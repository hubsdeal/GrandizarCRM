using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductMasterTagSettingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductMasterTagSettingForViewDto>> GetAll(GetAllProductMasterTagSettingsInput input);

        Task<GetProductMasterTagSettingForViewDto> GetProductMasterTagSettingForView(long id);

        Task<GetProductMasterTagSettingForEditOutput> GetProductMasterTagSettingForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductMasterTagSettingDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductMasterTagSettingsToExcel(GetAllProductMasterTagSettingsForExcelInput input);

        Task<PagedResultDto<ProductMasterTagSettingProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductMasterTagSettingMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

    }
}