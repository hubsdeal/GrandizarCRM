using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IBusinessAccountTeamsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBusinessAccountTeamForViewDto>> GetAll(GetAllBusinessAccountTeamsInput input);

        Task<GetBusinessAccountTeamForViewDto> GetBusinessAccountTeamForView(long id);

        Task<GetBusinessAccountTeamForEditOutput> GetBusinessAccountTeamForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBusinessAccountTeamDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetBusinessAccountTeamsToExcel(GetAllBusinessAccountTeamsForExcelInput input);

        Task<PagedResultDto<BusinessAccountTeamBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<BusinessAccountTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

    }
}