using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductReviewsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductReviewForViewDto>> GetAll(GetAllProductReviewsInput input);

        Task<GetProductReviewForViewDto> GetProductReviewForView(long id);

        Task<GetProductReviewForEditOutput> GetProductReviewForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductReviewDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductReviewsToExcel(GetAllProductReviewsForExcelInput input);

        Task<PagedResultDto<ProductReviewContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductReviewProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductReviewStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductReviewRatingLikeLookupTableDto>> GetAllRatingLikeForLookupTable(GetAllForLookupTableInput input);

    }
}