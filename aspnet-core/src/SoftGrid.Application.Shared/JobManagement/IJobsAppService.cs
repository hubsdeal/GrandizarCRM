using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.JobManagement
{
    public interface IJobsAppService : IApplicationService
    {
        Task<PagedResultDto<GetJobForViewDto>> GetAll(GetAllJobsInput input);

        Task<GetJobForViewDto> GetJobForView(long id);

        Task<GetJobForEditOutput> GetJobForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditJobDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetJobsToExcel(GetAllJobsForExcelInput input);

        Task<PagedResultDto<JobMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<JobMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<JobProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<JobCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<JobBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<JobCountryLookupTableDto>> GetAllCountryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<JobStateLookupTableDto>> GetAllStateForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<JobCityLookupTableDto>> GetAllCityForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<JobJobStatusTypeLookupTableDto>> GetAllJobStatusTypeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<JobStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}