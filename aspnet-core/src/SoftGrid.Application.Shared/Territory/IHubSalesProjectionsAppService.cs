using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Territory
{
    public interface IHubSalesProjectionsAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubSalesProjectionForViewDto>> GetAll(GetAllHubSalesProjectionsInput input);

        Task<GetHubSalesProjectionForViewDto> GetHubSalesProjectionForView(long id);

        Task<GetHubSalesProjectionForEditOutput> GetHubSalesProjectionForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubSalesProjectionDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubSalesProjectionsToExcel(GetAllHubSalesProjectionsForExcelInput input);

        Task<PagedResultDto<HubSalesProjectionHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubSalesProjectionProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubSalesProjectionStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubSalesProjectionCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input);

    }
}