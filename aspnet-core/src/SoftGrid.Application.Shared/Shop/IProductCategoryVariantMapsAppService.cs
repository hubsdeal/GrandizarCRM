using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductCategoryVariantMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductCategoryVariantMapForViewDto>> GetAll(GetAllProductCategoryVariantMapsInput input);

        Task<GetProductCategoryVariantMapForViewDto> GetProductCategoryVariantMapForView(long id);

        Task<GetProductCategoryVariantMapForEditOutput> GetProductCategoryVariantMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductCategoryVariantMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductCategoryVariantMapsToExcel(GetAllProductCategoryVariantMapsForExcelInput input);

        Task<PagedResultDto<ProductCategoryVariantMapProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductCategoryVariantMapProductVariantCategoryLookupTableDto>> GetAllProductVariantCategoryForLookupTable(GetAllForLookupTableInput input);

    }
}