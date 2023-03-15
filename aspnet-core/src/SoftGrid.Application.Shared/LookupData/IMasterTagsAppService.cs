using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using System.Collections.Generic;

namespace SoftGrid.LookupData
{
    public interface IMasterTagsAppService : IApplicationService
    {
        Task<PagedResultDto<GetMasterTagForViewDto>> GetAll(GetAllMasterTagsInput input);

        Task<GetMasterTagForViewDto> GetMasterTagForView(long id);

        Task<GetMasterTagForEditOutput> GetMasterTagForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditMasterTagDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetMasterTagsToExcel(GetAllMasterTagsForExcelInput input);

        Task<List<MasterTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForTableDropdown();

        Task<PagedResultDto<MasterTagMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input);

    }
}