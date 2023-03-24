using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;
using System.Collections.Generic;

namespace SoftGrid.TaskManagement
{
    public interface ITaskEventsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTaskEventForViewDto>> GetAll(GetAllTaskEventsInput input);

        Task<GetTaskEventForViewDto> GetTaskEventForView(long id);

        Task<GetTaskEventForEditOutput> GetTaskEventForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditTaskEventDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetTaskEventsToExcel(GetAllTaskEventsForExcelInput input);

        Task<List<TaskEventTaskStatusLookupTableDto>> GetAllTaskStatusForTableDropdown();

    }
}