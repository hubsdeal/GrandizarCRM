using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.SalesLeadManagement
{
    public interface ILeadPipelineStatusesAppService : IApplicationService
    {
        Task<PagedResultDto<GetLeadPipelineStatusForViewDto>> GetAll(GetAllLeadPipelineStatusesInput input);

        Task<GetLeadPipelineStatusForViewDto> GetLeadPipelineStatusForView(long id);

        Task<GetLeadPipelineStatusForEditOutput> GetLeadPipelineStatusForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditLeadPipelineStatusDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetLeadPipelineStatusesToExcel(GetAllLeadPipelineStatusesForExcelInput input);

        Task<PagedResultDto<LeadPipelineStatusLeadLookupTableDto>> GetAllLeadForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadPipelineStatusLeadPipelineStageLookupTableDto>> GetAllLeadPipelineStageForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadPipelineStatusEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

    }
}