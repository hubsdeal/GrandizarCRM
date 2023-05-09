using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IShoppingCartsAppService : IApplicationService
    {
        Task<PagedResultDto<GetShoppingCartForViewDto>> GetAll(GetAllShoppingCartsInput input);

        Task<GetShoppingCartForViewDto> GetShoppingCartForView(long id);

        Task<GetShoppingCartForEditOutput> GetShoppingCartForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditShoppingCartDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetShoppingCartsToExcel(GetAllShoppingCartsForExcelInput input);

        Task<PagedResultDto<ShoppingCartContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ShoppingCartOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ShoppingCartStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ShoppingCartProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ShoppingCartCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input);

    }
}