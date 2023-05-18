using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.TaskManagement
{
    public interface ITaskDocumentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTaskDocumentForViewDto>> GetAll(GetAllTaskDocumentsInput input);

        Task<GetTaskDocumentForViewDto> GetTaskDocumentForView(long id);

        Task<GetTaskDocumentForEditOutput> GetTaskDocumentForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditTaskDocumentDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetTaskDocumentsToExcel(GetAllTaskDocumentsForExcelInput input);

        Task<PagedResultDto<TaskDocumentTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TaskDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input);

    }
}