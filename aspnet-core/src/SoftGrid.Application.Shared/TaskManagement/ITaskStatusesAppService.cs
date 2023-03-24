using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.TaskManagement
{
    public interface ITaskStatusesAppService : IApplicationService
    {
        Task<PagedResultDto<GetTaskStatusForViewDto>> GetAll(GetAllTaskStatusesInput input);

        Task<GetTaskStatusForViewDto> GetTaskStatusForView(long id);

        Task<GetTaskStatusForEditOutput> GetTaskStatusForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditTaskStatusDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetTaskStatusesToExcel(GetAllTaskStatusesForExcelInput input);

    }
}