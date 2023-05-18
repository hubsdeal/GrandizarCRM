using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductDocumentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductDocumentForViewDto>> GetAll(GetAllProductDocumentsInput input);

        Task<GetProductDocumentForEditOutput> GetProductDocumentForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductDocumentDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductDocumentsToExcel(GetAllProductDocumentsForExcelInput input);

        Task<PagedResultDto<ProductDocumentProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input);

    }
}