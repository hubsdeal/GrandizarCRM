using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.OrderManagement
{
    public interface IOrderStatusesAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderStatusForViewDto>> GetAll(GetAllOrderStatusesInput input);

        Task<GetOrderStatusForViewDto> GetOrderStatusForView(long id);

        Task<GetOrderStatusForEditOutput> GetOrderStatusForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditOrderStatusDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetOrderStatusesToExcel(GetAllOrderStatusesForExcelInput input);

        Task<PagedResultDto<OrderStatusRoleLookupTableDto>> GetAllRoleForLookupTable(GetAllForLookupTableInput input);

    }
}