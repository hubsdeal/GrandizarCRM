using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreTagsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreTagForViewDto>> GetAll(GetAllStoreTagsInput input);

        Task<GetStoreTagForViewDto> GetStoreTagForView(long id);

        Task<GetStoreTagForEditOutput> GetStoreTagForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreTagDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreTagsToExcel(GetAllStoreTagsForExcelInput input);

        Task<PagedResultDto<StoreTagStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreTagMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input);

    }
}