using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreReviewsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreReviewForViewDto>> GetAll(GetAllStoreReviewsInput input);

        Task<GetStoreReviewForViewDto> GetStoreReviewForView(long id);

        Task<GetStoreReviewForEditOutput> GetStoreReviewForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreReviewDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreReviewsToExcel(GetAllStoreReviewsForExcelInput input);

        Task<PagedResultDto<StoreReviewStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreReviewContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreReviewRatingLikeLookupTableDto>> GetAllRatingLikeForLookupTable(GetAllForLookupTableInput input);

    }
}