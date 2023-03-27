using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.SalesLeadManagement
{
    public interface ILeadsAppService : IApplicationService
    {
        Task<PagedResultDto<GetLeadForViewDto>> GetAll(GetAllLeadsInput input);

        Task<GetLeadForViewDto> GetLeadForView(long id);

        Task<GetLeadForEditOutput> GetLeadForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditLeadDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetLeadsToExcel(GetAllLeadsForExcelInput input);

        Task<PagedResultDto<LeadContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadLeadSourceLookupTableDto>> GetAllLeadSourceForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadLeadPipelineStageLookupTableDto>> GetAllLeadPipelineStageForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input);

    }
}