using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductAndGiftCardMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductAndGiftCardMapForViewDto>> GetAll(GetAllProductAndGiftCardMapsInput input);

        Task<GetProductAndGiftCardMapForViewDto> GetProductAndGiftCardMapForView(long id);

        Task<GetProductAndGiftCardMapForEditOutput> GetProductAndGiftCardMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductAndGiftCardMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductAndGiftCardMapsToExcel(GetAllProductAndGiftCardMapsForExcelInput input);

        Task<PagedResultDto<ProductAndGiftCardMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductAndGiftCardMapCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input);

    }
}