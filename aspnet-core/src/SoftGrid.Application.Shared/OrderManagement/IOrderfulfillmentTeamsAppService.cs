using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.OrderManagement
{
    public interface IOrderfulfillmentTeamsAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderfulfillmentTeamForViewDto>> GetAll(GetAllOrderfulfillmentTeamsInput input);

        Task<GetOrderfulfillmentTeamForViewDto> GetOrderfulfillmentTeamForView(long id);

        Task<GetOrderfulfillmentTeamForEditOutput> GetOrderfulfillmentTeamForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditOrderfulfillmentTeamDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetOrderfulfillmentTeamsToExcel(GetAllOrderfulfillmentTeamsForExcelInput input);

        Task<PagedResultDto<OrderfulfillmentTeamOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderfulfillmentTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderfulfillmentTeamContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderfulfillmentTeamUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

    }
}