using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductVariantCategoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductVariantCategoryForViewDto>> GetAll(GetAllProductVariantCategoriesInput input);

        Task<GetProductVariantCategoryForViewDto> GetProductVariantCategoryForView(long id);

        Task<GetProductVariantCategoryForEditOutput> GetProductVariantCategoryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductVariantCategoryDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductVariantCategoriesToExcel(GetAllProductVariantCategoriesForExcelInput input);

        Task<PagedResultDto<ProductVariantCategoryStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}