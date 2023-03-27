using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.SalesLeadManagement
{
    public interface ILeadTasksAppService : IApplicationService
    {
        Task<PagedResultDto<GetLeadTaskForViewDto>> GetAll(GetAllLeadTasksInput input);

        Task<GetLeadTaskForViewDto> GetLeadTaskForView(long id);

        Task<GetLeadTaskForEditOutput> GetLeadTaskForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditLeadTaskDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetLeadTasksToExcel(GetAllLeadTasksForExcelInput input);

        Task<PagedResultDto<LeadTaskLeadLookupTableDto>> GetAllLeadForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadTaskTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input);

    }
}