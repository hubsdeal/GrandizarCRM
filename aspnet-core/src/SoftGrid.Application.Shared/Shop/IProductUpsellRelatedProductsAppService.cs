using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductUpsellRelatedProductsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductUpsellRelatedProductForViewDto>> GetAll(GetAllProductUpsellRelatedProductsInput input);

        Task<GetProductUpsellRelatedProductForViewDto> GetProductUpsellRelatedProductForView(long id);

        Task<GetProductUpsellRelatedProductForEditOutput> GetProductUpsellRelatedProductForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductUpsellRelatedProductDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductUpsellRelatedProductsToExcel(GetAllProductUpsellRelatedProductsForExcelInput input);

        Task<PagedResultDto<ProductUpsellRelatedProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

    }
}