using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IEmployeesAppService : IApplicationService
    {
        Task<PagedResultDto<GetEmployeeForViewDto>> GetAll(GetAllEmployeesInput input);

        Task<GetEmployeeForViewDto> GetEmployeeForView(long id);

        Task<GetEmployeeForEditOutput> GetEmployeeForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditEmployeeDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetEmployeesToExcel(GetAllEmployeesForExcelInput input);

        Task<PagedResultDto<EmployeeStateLookupTableDto>> GetAllStateForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<EmployeeCountryLookupTableDto>> GetAllCountryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<EmployeeContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

    }
}