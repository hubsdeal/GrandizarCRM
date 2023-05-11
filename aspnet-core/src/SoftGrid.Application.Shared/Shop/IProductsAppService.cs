using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input);

        Task<GetProductForViewDto> GetProductForView(long id);

        Task<GetProductForEditOutput> GetProductForEdit(EntityDto<long> input);

        Task<long> CreateOrEdit(CreateOrEditProductDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input);

        Task<PagedResultDto<ProductProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductMeasurementUnitLookupTableDto>> GetAllMeasurementUnitForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductRatingLikeLookupTableDto>> GetAllRatingLikeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}