using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.JobManagement
{
    public interface IJobMasterTagSettingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetJobMasterTagSettingForViewDto>> GetAll(GetAllJobMasterTagSettingsInput input);

        Task<GetJobMasterTagSettingForViewDto> GetJobMasterTagSettingForView(long id);

        Task<GetJobMasterTagSettingForEditOutput> GetJobMasterTagSettingForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditJobMasterTagSettingDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetJobMasterTagSettingsToExcel(GetAllJobMasterTagSettingsForExcelInput input);

        Task<PagedResultDto<JobMasterTagSettingMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<JobMasterTagSettingMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

    }
}