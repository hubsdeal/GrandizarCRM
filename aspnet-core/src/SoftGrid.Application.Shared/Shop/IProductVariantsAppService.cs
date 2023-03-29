using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductVariantsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductVariantForViewDto>> GetAll(GetAllProductVariantsInput input);

        Task<GetProductVariantForViewDto> GetProductVariantForView(long id);

        Task<GetProductVariantForEditOutput> GetProductVariantForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductVariantDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductVariantsToExcel(GetAllProductVariantsForExcelInput input);

        Task<PagedResultDto<ProductVariantProductVariantCategoryLookupTableDto>> GetAllProductVariantCategoryForLookupTable(GetAllForLookupTableInput input);

    }
}