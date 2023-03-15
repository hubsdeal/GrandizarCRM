using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface IDocumentTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetDocumentTypeForViewDto>> GetAll(GetAllDocumentTypesInput input);

        Task<GetDocumentTypeForViewDto> GetDocumentTypeForView(long id);

        Task<GetDocumentTypeForEditOutput> GetDocumentTypeForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDocumentTypeDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetDocumentTypesToExcel(GetAllDocumentTypesForExcelInput input);

    }
}