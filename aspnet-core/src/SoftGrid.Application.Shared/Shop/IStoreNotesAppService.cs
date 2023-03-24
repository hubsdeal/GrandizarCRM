using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreNotesAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreNoteForViewDto>> GetAll(GetAllStoreNotesInput input);

        Task<GetStoreNoteForViewDto> GetStoreNoteForView(long id);

        Task<GetStoreNoteForEditOutput> GetStoreNoteForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreNoteDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreNotesToExcel(GetAllStoreNotesForExcelInput input);

        Task<PagedResultDto<StoreNoteStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}