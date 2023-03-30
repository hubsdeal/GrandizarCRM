using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductCategoryMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductCategoryMapForViewDto>> GetAll(GetAllProductCategoryMapsInput input);

        Task<GetProductCategoryMapForViewDto> GetProductCategoryMapForView(long id);

        Task<GetProductCategoryMapForEditOutput> GetProductCategoryMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductCategoryMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductCategoryMapsToExcel(GetAllProductCategoryMapsForExcelInput input);

        Task<PagedResultDto<ProductCategoryMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductCategoryMapProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input);

    }
}