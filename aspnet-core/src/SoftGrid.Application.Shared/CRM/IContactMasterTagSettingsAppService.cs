using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IContactMasterTagSettingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetContactMasterTagSettingForViewDto>> GetAll(GetAllContactMasterTagSettingsInput input);

        Task<GetContactMasterTagSettingForViewDto> GetContactMasterTagSettingForView(long id);

        Task<GetContactMasterTagSettingForEditOutput> GetContactMasterTagSettingForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditContactMasterTagSettingDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetContactMasterTagSettingsToExcel(GetAllContactMasterTagSettingsForExcelInput input);

        Task<PagedResultDto<ContactMasterTagSettingMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ContactMasterTagSettingMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

    }
}