using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreProductCategoryMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreProductCategoryMapForViewDto>> GetAll(GetAllStoreProductCategoryMapsInput input);

        Task<GetStoreProductCategoryMapForViewDto> GetStoreProductCategoryMapForView(long id);

        Task<GetStoreProductCategoryMapForEditOutput> GetStoreProductCategoryMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreProductCategoryMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreProductCategoryMapsToExcel(GetAllStoreProductCategoryMapsForExcelInput input);

        Task<PagedResultDto<StoreProductCategoryMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreProductCategoryMapProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input);

    }
}