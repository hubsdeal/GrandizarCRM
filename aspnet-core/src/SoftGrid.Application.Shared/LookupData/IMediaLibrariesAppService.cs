using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface IMediaLibrariesAppService : IApplicationService
    {
        Task<PagedResultDto<GetMediaLibraryForViewDto>> GetAll(GetAllMediaLibrariesInput input);

        Task<GetMediaLibraryForViewDto> GetMediaLibraryForView(long id);

        Task<GetMediaLibraryForEditOutput> GetMediaLibraryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditMediaLibraryDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetMediaLibrariesToExcel(GetAllMediaLibrariesForExcelInput input);

        Task<PagedResultDto<MediaLibraryMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<MediaLibraryMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input);

    }
}