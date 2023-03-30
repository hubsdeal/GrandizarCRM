using SoftGrid.CRM;
using SoftGrid.Shop;
using SoftGrid.LookupData;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.Shop.Exporting;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.Shop
{
    [AbpAuthorize(AppPermissions.Pages_ProductReviewFeedbacks)]
    public class ProductReviewFeedbacksAppService : SoftGridAppServiceBase, IProductReviewFeedbacksAppService
    {
        private readonly IRepository<ProductReviewFeedback, long> _productReviewFeedbackRepository;
        private readonly IProductReviewFeedbacksExcelExporter _productReviewFeedbacksExcelExporter;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<ProductReview, long> _lookup_productReviewRepository;
        private readonly IRepository<RatingLike, long> _lookup_ratingLikeRepository;

        public ProductReviewFeedbacksAppService(IRepository<ProductReviewFeedback, long> productReviewFeedbackRepository, IProductReviewFeedbacksExcelExporter productReviewFeedbacksExcelExporter, IRepository<Contact, long> lookup_contactRepository, IRepository<ProductReview, long> lookup_productReviewRepository, IRepository<RatingLike, long> lookup_ratingLikeRepository)
        {
            _productReviewFeedbackRepository = productReviewFeedbackRepository;
            _productReviewFeedbacksExcelExporter = productReviewFeedbacksExcelExporter;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_productReviewRepository = lookup_productReviewRepository;
            _lookup_ratingLikeRepository = lookup_ratingLikeRepository;

        }

        public async Task<PagedResultDto<GetProductReviewFeedbackForViewDto>> GetAll(GetAllProductReviewFeedbacksInput input)
        {

            var filteredProductReviewFeedbacks = _productReviewFeedbackRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.ProductReviewFk)
                        .Include(e => e.RatingLikeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReplyText.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReplyTextFilter), e => e.ReplyText.Contains(input.ReplyTextFilter))
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductReviewReviewInfoFilter), e => e.ProductReviewFk != null && e.ProductReviewFk.ReviewInfo == input.ProductReviewReviewInfoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RatingLikeNameFilter), e => e.RatingLikeFk != null && e.RatingLikeFk.Name == input.RatingLikeNameFilter);

            var pagedAndFilteredProductReviewFeedbacks = filteredProductReviewFeedbacks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productReviewFeedbacks = from o in pagedAndFilteredProductReviewFeedbacks
                                         join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         join o2 in _lookup_productReviewRepository.GetAll() on o.ProductReviewId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()

                                         join o3 in _lookup_ratingLikeRepository.GetAll() on o.RatingLikeId equals o3.Id into j3
                                         from s3 in j3.DefaultIfEmpty()

                                         select new
                                         {

                                             o.ReplyText,
                                             o.Published,
                                             Id = o.Id,
                                             ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                                             ProductReviewReviewInfo = s2 == null || s2.ReviewInfo == null ? "" : s2.ReviewInfo.ToString(),
                                             RatingLikeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                         };

            var totalCount = await filteredProductReviewFeedbacks.CountAsync();

            var dbList = await productReviewFeedbacks.ToListAsync();
            var results = new List<GetProductReviewFeedbackForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductReviewFeedbackForViewDto()
                {
                    ProductReviewFeedback = new ProductReviewFeedbackDto
                    {

                        ReplyText = o.ReplyText,
                        Published = o.Published,
                        Id = o.Id,
                    },
                    ContactFullName = o.ContactFullName,
                    ProductReviewReviewInfo = o.ProductReviewReviewInfo,
                    RatingLikeName = o.RatingLikeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductReviewFeedbackForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductReviewFeedbackForViewDto> GetProductReviewFeedbackForView(long id)
        {
            var productReviewFeedback = await _productReviewFeedbackRepository.GetAsync(id);

            var output = new GetProductReviewFeedbackForViewDto { ProductReviewFeedback = ObjectMapper.Map<ProductReviewFeedbackDto>(productReviewFeedback) };

            if (output.ProductReviewFeedback.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ProductReviewFeedback.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ProductReviewFeedback.ProductReviewId != null)
            {
                var _lookupProductReview = await _lookup_productReviewRepository.FirstOrDefaultAsync((long)output.ProductReviewFeedback.ProductReviewId);
                output.ProductReviewReviewInfo = _lookupProductReview?.ReviewInfo?.ToString();
            }

            if (output.ProductReviewFeedback.RatingLikeId != null)
            {
                var _lookupRatingLike = await _lookup_ratingLikeRepository.FirstOrDefaultAsync((long)output.ProductReviewFeedback.RatingLikeId);
                output.RatingLikeName = _lookupRatingLike?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductReviewFeedbacks_Edit)]
        public async Task<GetProductReviewFeedbackForEditOutput> GetProductReviewFeedbackForEdit(EntityDto<long> input)
        {
            var productReviewFeedback = await _productReviewFeedbackRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductReviewFeedbackForEditOutput { ProductReviewFeedback = ObjectMapper.Map<CreateOrEditProductReviewFeedbackDto>(productReviewFeedback) };

            if (output.ProductReviewFeedback.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ProductReviewFeedback.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ProductReviewFeedback.ProductReviewId != null)
            {
                var _lookupProductReview = await _lookup_productReviewRepository.FirstOrDefaultAsync((long)output.ProductReviewFeedback.ProductReviewId);
                output.ProductReviewReviewInfo = _lookupProductReview?.ReviewInfo?.ToString();
            }

            if (output.ProductReviewFeedback.RatingLikeId != null)
            {
                var _lookupRatingLike = await _lookup_ratingLikeRepository.FirstOrDefaultAsync((long)output.ProductReviewFeedback.RatingLikeId);
                output.RatingLikeName = _lookupRatingLike?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductReviewFeedbackDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_ProductReviewFeedbacks_Create)]
        protected virtual async Task Create(CreateOrEditProductReviewFeedbackDto input)
        {
            var productReviewFeedback = ObjectMapper.Map<ProductReviewFeedback>(input);

            if (AbpSession.TenantId != null)
            {
                productReviewFeedback.TenantId = (int?)AbpSession.TenantId;
            }

            await _productReviewFeedbackRepository.InsertAsync(productReviewFeedback);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductReviewFeedbacks_Edit)]
        protected virtual async Task Update(CreateOrEditProductReviewFeedbackDto input)
        {
            var productReviewFeedback = await _productReviewFeedbackRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productReviewFeedback);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductReviewFeedbacks_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productReviewFeedbackRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductReviewFeedbacksToExcel(GetAllProductReviewFeedbacksForExcelInput input)
        {

            var filteredProductReviewFeedbacks = _productReviewFeedbackRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.ProductReviewFk)
                        .Include(e => e.RatingLikeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReplyText.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReplyTextFilter), e => e.ReplyText.Contains(input.ReplyTextFilter))
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductReviewReviewInfoFilter), e => e.ProductReviewFk != null && e.ProductReviewFk.ReviewInfo == input.ProductReviewReviewInfoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RatingLikeNameFilter), e => e.RatingLikeFk != null && e.RatingLikeFk.Name == input.RatingLikeNameFilter);

            var query = (from o in filteredProductReviewFeedbacks
                         join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productReviewRepository.GetAll() on o.ProductReviewId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_ratingLikeRepository.GetAll() on o.RatingLikeId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetProductReviewFeedbackForViewDto()
                         {
                             ProductReviewFeedback = new ProductReviewFeedbackDto
                             {
                                 ReplyText = o.ReplyText,
                                 Published = o.Published,
                                 Id = o.Id
                             },
                             ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                             ProductReviewReviewInfo = s2 == null || s2.ReviewInfo == null ? "" : s2.ReviewInfo.ToString(),
                             RatingLikeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var productReviewFeedbackListDtos = await query.ToListAsync();

            return _productReviewFeedbacksExcelExporter.ExportToFile(productReviewFeedbackListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductReviewFeedbacks)]
        public async Task<PagedResultDto<ProductReviewFeedbackContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductReviewFeedbackContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new ProductReviewFeedbackContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<ProductReviewFeedbackContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductReviewFeedbacks)]
        public async Task<PagedResultDto<ProductReviewFeedbackProductReviewLookupTableDto>> GetAllProductReviewForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productReviewRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.ReviewInfo != null && e.ReviewInfo.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productReviewList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductReviewFeedbackProductReviewLookupTableDto>();
            foreach (var productReview in productReviewList)
            {
                lookupTableDtoList.Add(new ProductReviewFeedbackProductReviewLookupTableDto
                {
                    Id = productReview.Id,
                    DisplayName = productReview.ReviewInfo?.ToString()
                });
            }

            return new PagedResultDto<ProductReviewFeedbackProductReviewLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductReviewFeedbacks)]
        public async Task<PagedResultDto<ProductReviewFeedbackRatingLikeLookupTableDto>> GetAllRatingLikeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_ratingLikeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var ratingLikeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductReviewFeedbackRatingLikeLookupTableDto>();
            foreach (var ratingLike in ratingLikeList)
            {
                lookupTableDtoList.Add(new ProductReviewFeedbackRatingLikeLookupTableDto
                {
                    Id = ratingLike.Id,
                    DisplayName = ratingLike.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductReviewFeedbackRatingLikeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}