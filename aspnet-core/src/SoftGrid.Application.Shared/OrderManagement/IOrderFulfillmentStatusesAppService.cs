using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.OrderManagement
{
    public interface IOrderFulfillmentStatusesAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderFulfillmentStatusForViewDto>> GetAll(GetAllOrderFulfillmentStatusesInput input);

        Task<GetOrderFulfillmentStatusForViewDto> GetOrderFulfillmentStatusForView(long id);

        Task<GetOrderFulfillmentStatusForEditOutput> GetOrderFulfillmentStatusForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditOrderFulfillmentStatusDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetOrderFulfillmentStatusesToExcel(GetAllOrderFulfillmentStatusesForExcelInput input);

        Task<PagedResultDto<OrderFulfillmentStatusOrderStatusLookupTableDto>> GetAllOrderStatusForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderFulfillmentStatusOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderFulfillmentStatusEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

    }
}