using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.OrderManagement
{
    public interface IOrdersAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderForViewDto>> GetAll(GetAllOrdersInput input);

        Task<GetOrderForViewDto> GetOrderForView(long id);

        Task<GetOrderForEditOutput> GetOrderForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditOrderDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetOrdersToExcel(GetAllOrdersForExcelInput input);

        Task<PagedResultDto<OrderStateLookupTableDto>> GetAllStateForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderCountryLookupTableDto>> GetAllCountryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderOrderStatusLookupTableDto>> GetAllOrderStatusForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderOrderSalesChannelLookupTableDto>> GetAllOrderSalesChannelForLookupTable(GetAllForLookupTableInput input);

    }
}