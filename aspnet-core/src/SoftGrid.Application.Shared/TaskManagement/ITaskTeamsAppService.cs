using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.TaskManagement
{
    public interface ITaskTeamsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTaskTeamForViewDto>> GetAll(GetAllTaskTeamsInput input);

        Task<GetTaskTeamForViewDto> GetTaskTeamForView(long id);

        Task<GetTaskTeamForEditOutput> GetTaskTeamForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditTaskTeamDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetTaskTeamsToExcel(GetAllTaskTeamsForExcelInput input);

        Task<PagedResultDto<TaskTeamTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TaskTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TaskTeamContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

    }
}