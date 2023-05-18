using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.JobManagement
{
    public interface IJobDocumentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetJobDocumentForViewDto>> GetAll(GetAllJobDocumentsInput input);

        Task<GetJobDocumentForViewDto> GetJobDocumentForView(long id);

        Task<GetJobDocumentForEditOutput> GetJobDocumentForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditJobDocumentDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetJobDocumentsToExcel(GetAllJobDocumentsForExcelInput input);

        Task<PagedResultDto<JobDocumentJobLookupTableDto>> GetAllJobForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<JobDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input);

    }
}