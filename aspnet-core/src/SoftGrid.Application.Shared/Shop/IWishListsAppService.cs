using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IWishListsAppService : IApplicationService
    {
        Task<PagedResultDto<GetWishListForViewDto>> GetAll(GetAllWishListsInput input);

        Task<GetWishListForViewDto> GetWishListForView(long id);

        Task<GetWishListForEditOutput> GetWishListForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditWishListDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetWishListsToExcel(GetAllWishListsForExcelInput input);

        Task<PagedResultDto<WishListContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<WishListProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<WishListStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}