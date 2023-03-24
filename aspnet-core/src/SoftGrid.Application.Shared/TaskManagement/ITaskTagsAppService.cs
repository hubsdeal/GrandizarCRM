using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.TaskManagement
{
    public interface ITaskTagsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTaskTagForViewDto>> GetAll(GetAllTaskTagsInput input);

        Task<GetTaskTagForViewDto> GetTaskTagForView(long id);

        Task<GetTaskTagForEditOutput> GetTaskTagForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditTaskTagDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetTaskTagsToExcel(GetAllTaskTagsForExcelInput input);

        Task<PagedResultDto<TaskTagTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TaskTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TaskTagMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input);

    }
}