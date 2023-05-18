using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IBusinessDocumentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBusinessDocumentForViewDto>> GetAll(GetAllBusinessDocumentsInput input);

        Task<GetBusinessDocumentForViewDto> GetBusinessDocumentForView(long id);

        Task<GetBusinessDocumentForEditOutput> GetBusinessDocumentForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBusinessDocumentDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetBusinessDocumentsToExcel(GetAllBusinessDocumentsForExcelInput input);

        Task<PagedResultDto<BusinessDocumentBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<BusinessDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input);

    }
}