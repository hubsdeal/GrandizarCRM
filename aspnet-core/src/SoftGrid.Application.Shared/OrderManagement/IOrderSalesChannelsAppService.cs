using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.OrderManagement
{
    public interface IOrderSalesChannelsAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderSalesChannelForViewDto>> GetAll(GetAllOrderSalesChannelsInput input);

        Task<GetOrderSalesChannelForViewDto> GetOrderSalesChannelForView(long id);

        Task<GetOrderSalesChannelForEditOutput> GetOrderSalesChannelForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditOrderSalesChannelDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetOrderSalesChannelsToExcel(GetAllOrderSalesChannelsForExcelInput input);

    }
}