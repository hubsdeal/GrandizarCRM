using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductNotesAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductNoteForViewDto>> GetAll(GetAllProductNotesInput input);

        Task<GetProductNoteForViewDto> GetProductNoteForView(long id);

        Task<GetProductNoteForEditOutput> GetProductNoteForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductNoteDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductNotesToExcel(GetAllProductNotesForExcelInput input);

        Task<PagedResultDto<ProductNoteProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

    }
}