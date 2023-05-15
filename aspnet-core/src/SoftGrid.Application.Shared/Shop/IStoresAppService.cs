using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;

namespace SoftGrid.Shop
{
    public interface IStoresAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreForViewDto>> GetAll(GetAllStoresInput input);

        Task<GetStoreForViewDto> GetStoreForView(long id);

        Task<GetStoreForEditOutput> GetStoreForEdit(EntityDto<long> input);

        Task<long?> CreateOrEdit(CreateOrEditStoreDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoresToExcel(GetAllStoresForExcelInput input);

        Task<PagedResultDto<StoreMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input);

        Task<List<StoreCountryLookupTableDto>> GetAllCountryForTableDropdown();

        Task<List<StoreStateLookupTableDto>> GetAllStateForTableDropdown();

        Task<List<StoreRatingLikeLookupTableDto>> GetAllRatingLikeForTableDropdown();

        Task<PagedResultDto<StoreMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input);

    }
}