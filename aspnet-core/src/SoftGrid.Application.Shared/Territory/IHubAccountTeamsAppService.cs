using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Territory
{
    public interface IHubAccountTeamsAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubAccountTeamForViewDto>> GetAll(GetAllHubAccountTeamsInput input);

        Task<GetHubAccountTeamForViewDto> GetHubAccountTeamForView(long id);

        Task<GetHubAccountTeamForEditOutput> GetHubAccountTeamForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubAccountTeamDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubAccountTeamsToExcel(GetAllHubAccountTeamsForExcelInput input);

        Task<PagedResultDto<HubAccountTeamHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubAccountTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubAccountTeamUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

    }
}