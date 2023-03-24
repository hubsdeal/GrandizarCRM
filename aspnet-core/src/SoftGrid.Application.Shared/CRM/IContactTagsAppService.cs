using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IContactTagsAppService : IApplicationService
    {
        Task<PagedResultDto<GetContactTagForViewDto>> GetAll(GetAllContactTagsInput input);

        Task<GetContactTagForViewDto> GetContactTagForView(long id);

        Task<GetContactTagForEditOutput> GetContactTagForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditContactTagDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetContactTagsToExcel(GetAllContactTagsForExcelInput input);

        Task<PagedResultDto<ContactTagContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ContactTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ContactTagMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input);

    }
}