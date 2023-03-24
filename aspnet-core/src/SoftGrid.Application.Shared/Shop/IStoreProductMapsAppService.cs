using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreProductMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreProductMapForViewDto>> GetAll(GetAllStoreProductMapsInput input);

        Task<GetStoreProductMapForViewDto> GetStoreProductMapForView(long id);

        Task<GetStoreProductMapForEditOutput> GetStoreProductMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreProductMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreProductMapsToExcel(GetAllStoreProductMapsForExcelInput input);

        Task<PagedResultDto<StoreProductMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreProductMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

    }
}