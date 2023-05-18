using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Territory
{
    public interface IHubDocumentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubDocumentForViewDto>> GetAll(GetAllHubDocumentsInput input);

        Task<GetHubDocumentForEditOutput> GetHubDocumentForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubDocumentDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubDocumentsToExcel(GetAllHubDocumentsForExcelInput input);

        Task<PagedResultDto<HubDocumentHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input);

    }
}