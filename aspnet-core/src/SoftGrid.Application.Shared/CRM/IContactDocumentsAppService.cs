using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IContactDocumentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetContactDocumentForViewDto>> GetAll(GetAllContactDocumentsInput input);

        Task<GetContactDocumentForEditOutput> GetContactDocumentForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditContactDocumentDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetContactDocumentsToExcel(GetAllContactDocumentsForExcelInput input);

        Task<PagedResultDto<ContactDocumentContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ContactDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input);

    }
}