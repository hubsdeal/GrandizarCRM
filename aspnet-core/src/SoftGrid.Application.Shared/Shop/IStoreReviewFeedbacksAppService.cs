using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreReviewFeedbacksAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreReviewFeedbackForViewDto>> GetAll(GetAllStoreReviewFeedbacksInput input);

        Task<GetStoreReviewFeedbackForViewDto> GetStoreReviewFeedbackForView(long id);

        Task<GetStoreReviewFeedbackForEditOutput> GetStoreReviewFeedbackForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreReviewFeedbackDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreReviewFeedbacksToExcel(GetAllStoreReviewFeedbacksForExcelInput input);

        Task<PagedResultDto<StoreReviewFeedbackStoreReviewLookupTableDto>> GetAllStoreReviewForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreReviewFeedbackContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreReviewFeedbackRatingLikeLookupTableDto>> GetAllRatingLikeForLookupTable(GetAllForLookupTableInput input);

    }
}