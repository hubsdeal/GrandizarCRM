using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.OrderManagement
{
    public interface IOrderTeamsAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderTeamForViewDto>> GetAll(GetAllOrderTeamsInput input);

        Task<GetOrderTeamForViewDto> GetOrderTeamForView(long id);

        Task<GetOrderTeamForEditOutput> GetOrderTeamForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditOrderTeamDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetOrderTeamsToExcel(GetAllOrderTeamsForExcelInput input);

        Task<PagedResultDto<OrderTeamOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

    }
}