using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.OrderManagement
{
    public interface IOrderDeliveryInfosAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderDeliveryInfoForViewDto>> GetAll(GetAllOrderDeliveryInfosInput input);

        Task<GetOrderDeliveryInfoForViewDto> GetOrderDeliveryInfoForView(long id);

        Task<GetOrderDeliveryInfoForEditOutput> GetOrderDeliveryInfoForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditOrderDeliveryInfoDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetOrderDeliveryInfosToExcel(GetAllOrderDeliveryInfosForExcelInput input);

        Task<PagedResultDto<OrderDeliveryInfoEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderDeliveryInfoOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input);

    }
}