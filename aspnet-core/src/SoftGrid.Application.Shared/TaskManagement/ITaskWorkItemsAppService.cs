using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.TaskManagement
{
    public interface ITaskWorkItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTaskWorkItemForViewDto>> GetAll(GetAllTaskWorkItemsInput input);

        Task<GetTaskWorkItemForViewDto> GetTaskWorkItemForView(long id);

        Task<GetTaskWorkItemForEditOutput> GetTaskWorkItemForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditTaskWorkItemDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetTaskWorkItemsToExcel(GetAllTaskWorkItemsForExcelInput input);

        Task<PagedResultDto<TaskWorkItemTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TaskWorkItemEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

    }
}