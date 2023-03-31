using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.OrderManagement
{
    public interface IOrderProductInfosAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderProductInfoForViewDto>> GetAll(GetAllOrderProductInfosInput input);

        Task<GetOrderProductInfoForViewDto> GetOrderProductInfoForView(long id);

        Task<GetOrderProductInfoForEditOutput> GetOrderProductInfoForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditOrderProductInfoDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetOrderProductInfosToExcel(GetAllOrderProductInfosForExcelInput input);

        Task<PagedResultDto<OrderProductInfoOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderProductInfoStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderProductInfoProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderProductInfoMeasurementUnitLookupTableDto>> GetAllMeasurementUnitForLookupTable(GetAllForLookupTableInput input);

    }
}