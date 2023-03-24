using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IEmployeeTagsAppService : IApplicationService
    {
        Task<PagedResultDto<GetEmployeeTagForViewDto>> GetAll(GetAllEmployeeTagsInput input);

        Task<GetEmployeeTagForViewDto> GetEmployeeTagForView(long id);

        Task<GetEmployeeTagForEditOutput> GetEmployeeTagForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditEmployeeTagDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetEmployeeTagsToExcel(GetAllEmployeeTagsForExcelInput input);

        Task<PagedResultDto<EmployeeTagEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<EmployeeTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<EmployeeTagMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input);

    }
}