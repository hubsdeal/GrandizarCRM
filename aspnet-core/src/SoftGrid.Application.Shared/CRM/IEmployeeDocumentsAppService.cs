using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IEmployeeDocumentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetEmployeeDocumentForViewDto>> GetAll(GetAllEmployeeDocumentsInput input);

        Task<GetEmployeeDocumentForEditOutput> GetEmployeeDocumentForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditEmployeeDocumentDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetEmployeeDocumentsToExcel(GetAllEmployeeDocumentsForExcelInput input);

        Task<PagedResultDto<EmployeeDocumentEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<EmployeeDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input);

    }
}