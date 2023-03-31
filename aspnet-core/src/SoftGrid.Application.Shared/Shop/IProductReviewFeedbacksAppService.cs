using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductReviewFeedbacksAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductReviewFeedbackForViewDto>> GetAll(GetAllProductReviewFeedbacksInput input);

        Task<GetProductReviewFeedbackForViewDto> GetProductReviewFeedbackForView(long id);

        Task<GetProductReviewFeedbackForEditOutput> GetProductReviewFeedbackForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductReviewFeedbackDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductReviewFeedbacksToExcel(GetAllProductReviewFeedbacksForExcelInput input);

        Task<PagedResultDto<ProductReviewFeedbackContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductReviewFeedbackProductReviewLookupTableDto>> GetAllProductReviewForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductReviewFeedbackRatingLikeLookupTableDto>> GetAllRatingLikeForLookupTable(GetAllForLookupTableInput input);

    }
}