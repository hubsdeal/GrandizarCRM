using SoftGrid.Shop;
using SoftGrid.CRM;
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
    [AbpAuthorize(AppPermissions.Pages_StoreReviews)]
    public class StoreReviewsAppService : SoftGridAppServiceBase, IStoreReviewsAppService
    {
        private readonly IRepository<StoreReview, long> _storeReviewRepository;
        private readonly IStoreReviewsExcelExporter _storeReviewsExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<RatingLike, long> _lookup_ratingLikeRepository;

        public StoreReviewsAppService(IRepository<StoreReview, long> storeReviewRepository, IStoreReviewsExcelExporter storeReviewsExcelExporter, IRepository<Store, long> lookup_storeRepository, IRepository<Contact, long> lookup_contactRepository, IRepository<RatingLike, long> lookup_ratingLikeRepository)
        {
            _storeReviewRepository = storeReviewRepository;
            _storeReviewsExcelExporter = storeReviewsExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_ratingLikeRepository = lookup_ratingLikeRepository;

        }

        public async Task<PagedResultDto<GetStoreReviewForViewDto>> GetAll(GetAllStoreReviewsInput input)
        {

            var filteredStoreReviews = _storeReviewRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.RatingLikeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReviewInfo.Contains(input.Filter) || e.PostTime.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReviewInfoFilter), e => e.ReviewInfo.Contains(input.ReviewInfoFilter))
                        .WhereIf(input.MinPostDateFilter != null, e => e.PostDate >= input.MinPostDateFilter)
                        .WhereIf(input.MaxPostDateFilter != null, e => e.PostDate <= input.MaxPostDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostTimeFilter), e => e.PostTime.Contains(input.PostTimeFilter))
                        .WhereIf(input.IsPublishFilter.HasValue && input.IsPublishFilter > -1, e => (input.IsPublishFilter == 1 && e.IsPublish) || (input.IsPublishFilter == 0 && !e.IsPublish))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RatingLikeNameFilter), e => e.RatingLikeFk != null && e.RatingLikeFk.Name == input.RatingLikeNameFilter);

            var pagedAndFilteredStoreReviews = filteredStoreReviews
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeReviews = from o in pagedAndFilteredStoreReviews
                               join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                               from s2 in j2.DefaultIfEmpty()

                               join o3 in _lookup_ratingLikeRepository.GetAll() on o.RatingLikeId equals o3.Id into j3
                               from s3 in j3.DefaultIfEmpty()

                               select new
                               {

                                   o.ReviewInfo,
                                   o.PostDate,
                                   o.PostTime,
                                   o.IsPublish,
                                   Id = o.Id,
                                   StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                   ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString(),
                                   RatingLikeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                               };

            var totalCount = await filteredStoreReviews.CountAsync();

            var dbList = await storeReviews.ToListAsync();
            var results = new List<GetStoreReviewForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreReviewForViewDto()
                {
                    StoreReview = new StoreReviewDto
                    {

                        ReviewInfo = o.ReviewInfo,
                        PostDate = o.PostDate,
                        PostTime = o.PostTime,
                        IsPublish = o.IsPublish,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName,
                    ContactFullName = o.ContactFullName,
                    RatingLikeName = o.RatingLikeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreReviewForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreReviewForViewDto> GetStoreReviewForView(long id)
        {
            var storeReview = await _storeReviewRepository.GetAsync(id);

            var output = new GetStoreReviewForViewDto { StoreReview = ObjectMapper.Map<StoreReviewDto>(storeReview) };

            if (output.StoreReview.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreReview.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreReview.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.StoreReview.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.StoreReview.RatingLikeId != null)
            {
                var _lookupRatingLike = await _lookup_ratingLikeRepository.FirstOrDefaultAsync((long)output.StoreReview.RatingLikeId);
                output.RatingLikeName = _lookupRatingLike?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreReviews_Edit)]
        public async Task<GetStoreReviewForEditOutput> GetStoreReviewForEdit(EntityDto<long> input)
        {
            var storeReview = await _storeReviewRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreReviewForEditOutput { StoreReview = ObjectMapper.Map<CreateOrEditStoreReviewDto>(storeReview) };

            if (output.StoreReview.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreReview.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreReview.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.StoreReview.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.StoreReview.RatingLikeId != null)
            {
                var _lookupRatingLike = await _lookup_ratingLikeRepository.FirstOrDefaultAsync((long)output.StoreReview.RatingLikeId);
                output.RatingLikeName = _lookupRatingLike?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreReviewDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreReviews_Create)]
        protected virtual async Task Create(CreateOrEditStoreReviewDto input)
        {
            var storeReview = ObjectMapper.Map<StoreReview>(input);

            if (AbpSession.TenantId != null)
            {
                storeReview.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeReviewRepository.InsertAsync(storeReview);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreReviews_Edit)]
        protected virtual async Task Update(CreateOrEditStoreReviewDto input)
        {
            var storeReview = await _storeReviewRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeReview);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreReviews_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeReviewRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreReviewsToExcel(GetAllStoreReviewsForExcelInput input)
        {

            var filteredStoreReviews = _storeReviewRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.RatingLikeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReviewInfo.Contains(input.Filter) || e.PostTime.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReviewInfoFilter), e => e.ReviewInfo.Contains(input.ReviewInfoFilter))
                        .WhereIf(input.MinPostDateFilter != null, e => e.PostDate >= input.MinPostDateFilter)
                        .WhereIf(input.MaxPostDateFilter != null, e => e.PostDate <= input.MaxPostDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostTimeFilter), e => e.PostTime.Contains(input.PostTimeFilter))
                        .WhereIf(input.IsPublishFilter.HasValue && input.IsPublishFilter > -1, e => (input.IsPublishFilter == 1 && e.IsPublish) || (input.IsPublishFilter == 0 && !e.IsPublish))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RatingLikeNameFilter), e => e.RatingLikeFk != null && e.RatingLikeFk.Name == input.RatingLikeNameFilter);

            var query = (from o in filteredStoreReviews
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_ratingLikeRepository.GetAll() on o.RatingLikeId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetStoreReviewForViewDto()
                         {
                             StoreReview = new StoreReviewDto
                             {
                                 ReviewInfo = o.ReviewInfo,
                                 PostDate = o.PostDate,
                                 PostTime = o.PostTime,
                                 IsPublish = o.IsPublish,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString(),
                             RatingLikeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var storeReviewListDtos = await query.ToListAsync();

            return _storeReviewsExcelExporter.ExportToFile(storeReviewListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreReviews)]
        public async Task<PagedResultDto<StoreReviewStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreReviewStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreReviewStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreReviewStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreReviews)]
        public async Task<PagedResultDto<StoreReviewContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreReviewContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new StoreReviewContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<StoreReviewContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreReviews)]
        public async Task<PagedResultDto<StoreReviewRatingLikeLookupTableDto>> GetAllRatingLikeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_ratingLikeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var ratingLikeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreReviewRatingLikeLookupTableDto>();
            foreach (var ratingLike in ratingLikeList)
            {
                lookupTableDtoList.Add(new StoreReviewRatingLikeLookupTableDto
                {
                    Id = ratingLike.Id,
                    DisplayName = ratingLike.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreReviewRatingLikeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}