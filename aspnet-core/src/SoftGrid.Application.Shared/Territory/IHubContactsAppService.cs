using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Territory
{
    public interface IHubContactsAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubContactForViewDto>> GetAll(GetAllHubContactsInput input);

        Task<GetHubContactForViewDto> GetHubContactForView(long id);

        Task<GetHubContactForEditOutput> GetHubContactForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubContactDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubContactsToExcel(GetAllHubContactsForExcelInput input);

        Task<PagedResultDto<HubContactHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubContactContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

    }
}