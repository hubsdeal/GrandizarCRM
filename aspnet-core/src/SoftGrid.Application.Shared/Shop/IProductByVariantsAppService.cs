using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductByVariantsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductByVariantForViewDto>> GetAll(GetAllProductByVariantsInput input);

        Task<GetProductByVariantForViewDto> GetProductByVariantForView(long id);

        Task<GetProductByVariantForEditOutput> GetProductByVariantForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductByVariantDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductByVariantsToExcel(GetAllProductByVariantsForExcelInput input);

        Task<PagedResultDto<ProductByVariantProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductByVariantProductVariantLookupTableDto>> GetAllProductVariantForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductByVariantProductVariantCategoryLookupTableDto>> GetAllProductVariantCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductByVariantMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input);

    }
}