using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.OrderManagement
{
    public interface IOrderPaymentInfosAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderPaymentInfoForViewDto>> GetAll(GetAllOrderPaymentInfosInput input);

        Task<GetOrderPaymentInfoForViewDto> GetOrderPaymentInfoForView(long id);

        Task<GetOrderPaymentInfoForEditOutput> GetOrderPaymentInfoForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditOrderPaymentInfoDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetOrderPaymentInfosToExcel(GetAllOrderPaymentInfosForExcelInput input);

        Task<PagedResultDto<OrderPaymentInfoOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderPaymentInfoCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderPaymentInfoPaymentTypeLookupTableDto>> GetAllPaymentTypeForLookupTable(GetAllForLookupTableInput input);

    }
}