using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.SalesLeadManagement
{
    public interface ILeadTagsAppService : IApplicationService
    {
        Task<PagedResultDto<GetLeadTagForViewDto>> GetAll(GetAllLeadTagsInput input);

        Task<GetLeadTagForViewDto> GetLeadTagForView(long id);

        Task<GetLeadTagForEditOutput> GetLeadTagForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditLeadTagDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetLeadTagsToExcel(GetAllLeadTagsForExcelInput input);

        Task<PagedResultDto<LeadTagLeadLookupTableDto>> GetAllLeadForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadTagMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input);

    }
}