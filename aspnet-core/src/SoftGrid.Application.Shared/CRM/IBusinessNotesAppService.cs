using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IBusinessNotesAppService : IApplicationService
    {
        Task<PagedResultDto<GetBusinessNoteForViewDto>> GetAll(GetAllBusinessNotesInput input);

        Task<GetBusinessNoteForViewDto> GetBusinessNoteForView(long id);

        Task<GetBusinessNoteForEditOutput> GetBusinessNoteForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBusinessNoteDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetBusinessNotesToExcel(GetAllBusinessNotesForExcelInput input);

        Task<PagedResultDto<BusinessNoteBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input);

    }
}