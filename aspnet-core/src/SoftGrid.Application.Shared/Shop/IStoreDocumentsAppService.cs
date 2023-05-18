using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreDocumentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreDocumentForViewDto>> GetAll(GetAllStoreDocumentsInput input);

        Task<GetStoreDocumentForEditOutput> GetStoreDocumentForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreDocumentDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreDocumentsToExcel(GetAllStoreDocumentsForExcelInput input);

        Task<PagedResultDto<StoreDocumentStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input);

    }
}