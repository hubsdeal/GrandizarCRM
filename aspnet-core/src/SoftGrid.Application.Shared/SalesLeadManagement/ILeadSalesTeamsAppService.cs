using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.SalesLeadManagement
{
    public interface ILeadSalesTeamsAppService : IApplicationService
    {
        Task<PagedResultDto<GetLeadSalesTeamForViewDto>> GetAll(GetAllLeadSalesTeamsInput input);

        Task<GetLeadSalesTeamForViewDto> GetLeadSalesTeamForView(long id);

        Task<GetLeadSalesTeamForEditOutput> GetLeadSalesTeamForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditLeadSalesTeamDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetLeadSalesTeamsToExcel(GetAllLeadSalesTeamsForExcelInput input);

        Task<PagedResultDto<LeadSalesTeamLeadLookupTableDto>> GetAllLeadForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadSalesTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

    }
}