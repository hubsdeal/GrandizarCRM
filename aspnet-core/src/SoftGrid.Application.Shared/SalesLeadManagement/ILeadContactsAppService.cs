using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.SalesLeadManagement
{
    public interface ILeadContactsAppService : IApplicationService
    {
        Task<PagedResultDto<GetLeadContactForViewDto>> GetAll(GetAllLeadContactsInput input);

        Task<GetLeadContactForViewDto> GetLeadContactForView(long id);

        Task<GetLeadContactForEditOutput> GetLeadContactForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditLeadContactDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetLeadContactsToExcel(GetAllLeadContactsForExcelInput input);

        Task<PagedResultDto<LeadContactLeadLookupTableDto>> GetAllLeadForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadContactContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

    }
}