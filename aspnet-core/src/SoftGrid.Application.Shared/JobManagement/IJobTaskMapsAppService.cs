using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.JobManagement
{
    public interface IJobTaskMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetJobTaskMapForViewDto>> GetAll(GetAllJobTaskMapsInput input);

        Task<GetJobTaskMapForViewDto> GetJobTaskMapForView(long id);

        Task<GetJobTaskMapForEditOutput> GetJobTaskMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditJobTaskMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetJobTaskMapsToExcel(GetAllJobTaskMapsForExcelInput input);

        Task<PagedResultDto<JobTaskMapJobLookupTableDto>> GetAllJobForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<JobTaskMapTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input);

    }
}