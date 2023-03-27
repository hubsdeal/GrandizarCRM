using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.SalesLeadManagement
{
    public interface ILeadNotesAppService : IApplicationService
    {
        Task<PagedResultDto<GetLeadNoteForViewDto>> GetAll(GetAllLeadNotesInput input);

        Task<GetLeadNoteForViewDto> GetLeadNoteForView(long id);

        Task<GetLeadNoteForEditOutput> GetLeadNoteForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditLeadNoteDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetLeadNotesToExcel(GetAllLeadNotesForExcelInput input);

        Task<PagedResultDto<LeadNoteLeadLookupTableDto>> GetAllLeadForLookupTable(GetAllForLookupTableInput input);

    }
}