using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;

namespace SoftGrid.CRM
{
    public interface IContactsAppService : IApplicationService
    {
        Task<PagedResultDto<GetContactForViewDto>> GetAll(GetAllContactsInput input);

        Task<GetContactForViewDto> GetContactForView(long id);

        Task<GetContactForEditOutput> GetContactForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditContactDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetContactsToExcel(GetAllContactsForExcelInput input);

        Task<PagedResultDto<ContactUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        Task<List<ContactCountryLookupTableDto>> GetAllCountryForTableDropdown();

        Task<List<ContactStateLookupTableDto>> GetAllStateForTableDropdown();

        Task<List<ContactMembershipTypeLookupTableDto>> GetAllMembershipTypeForTableDropdown();

    }
}