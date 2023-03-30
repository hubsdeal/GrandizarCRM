using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductCustomerStatsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductCustomerStatForViewDto>> GetAll(GetAllProductCustomerStatsInput input);

        Task<GetProductCustomerStatForViewDto> GetProductCustomerStatForView(long id);

        Task<GetProductCustomerStatForEditOutput> GetProductCustomerStatForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductCustomerStatDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductCustomerStatsToExcel(GetAllProductCustomerStatsForExcelInput input);

        Task<PagedResultDto<ProductCustomerStatProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductCustomerStatContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductCustomerStatStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductCustomerStatHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductCustomerStatSocialMediaLookupTableDto>> GetAllSocialMediaForLookupTable(GetAllForLookupTableInput input);

    }
}