using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.JobManagement
{
    public interface IJobTagsAppService : IApplicationService
    {
        Task<PagedResultDto<GetJobTagForViewDto>> GetAll(GetAllJobTagsInput input);

        Task<GetJobTagForViewDto> GetJobTagForView(long id);

        Task<GetJobTagForEditOutput> GetJobTagForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditJobTagDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetJobTagsToExcel(GetAllJobTagsForExcelInput input);

        Task<PagedResultDto<JobTagJobLookupTableDto>> GetAllJobForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<JobTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<JobTagMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input);

    }
}