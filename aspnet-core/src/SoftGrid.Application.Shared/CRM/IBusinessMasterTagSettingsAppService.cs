using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IBusinessMasterTagSettingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBusinessMasterTagSettingForViewDto>> GetAll(GetAllBusinessMasterTagSettingsInput input);

        Task<GetBusinessMasterTagSettingForViewDto> GetBusinessMasterTagSettingForView(long id);

        Task<GetBusinessMasterTagSettingForEditOutput> GetBusinessMasterTagSettingForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBusinessMasterTagSettingDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetBusinessMasterTagSettingsToExcel(GetAllBusinessMasterTagSettingsForExcelInput input);

        Task<PagedResultDto<BusinessMasterTagSettingMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<BusinessMasterTagSettingMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input);

    }
}