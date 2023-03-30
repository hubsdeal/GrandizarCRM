using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductCrossSellProductsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductCrossSellProductForViewDto>> GetAll(GetAllProductCrossSellProductsInput input);

        Task<GetProductCrossSellProductForViewDto> GetProductCrossSellProductForView(long id);

        Task<GetProductCrossSellProductForEditOutput> GetProductCrossSellProductForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductCrossSellProductDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductCrossSellProductsToExcel(GetAllProductCrossSellProductsForExcelInput input);

        Task<PagedResultDto<ProductCrossSellProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

    }
}