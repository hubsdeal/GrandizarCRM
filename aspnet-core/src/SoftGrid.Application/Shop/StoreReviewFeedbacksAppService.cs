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
    [AbpAuthorize(AppPermissions.Pages_StoreReviewFeedbacks)]
    public class StoreReviewFeedbacksAppService : SoftGridAppServiceBase, IStoreReviewFeedbacksAppService
    {
        private readonly IRepository<StoreReviewFeedback, long> _storeReviewFeedbackRepository;
        private readonly IStoreReviewFeedbacksExcelExporter _storeReviewFeedbacksExcelExporter;
        private readonly IRepository<StoreReview, long> _lookup_storeReviewRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<RatingLike, long> _lookup_ratingLikeRepository;

        public StoreReviewFeedbacksAppService(IRepository<StoreReviewFeedback, long> storeReviewFeedbackRepository, IStoreReviewFeedbacksExcelExporter storeReviewFeedbacksExcelExporter, IRepository<StoreReview, long> lookup_storeReviewRepository, IRepository<Contact, long> lookup_contactRepository, IRepository<RatingLike, long> lookup_ratingLikeRepository)
        {
            _storeReviewFeedbackRepository = storeReviewFeedbackRepository;
            _storeReviewFeedbacksExcelExporter = storeReviewFeedbacksExcelExporter;
            _lookup_storeReviewRepository = lookup_storeReviewRepository;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_ratingLikeRepository = lookup_ratingLikeRepository;

        }

        public async Task<PagedResultDto<GetStoreReviewFeedbackForViewDto>> GetAll(GetAllStoreReviewFeedbacksInput input)
        {

            var filteredStoreReviewFeedbacks = _storeReviewFeedbackRepository.GetAll()
                        .Include(e => e.StoreReviewFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.RatingLikeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReplyText.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReplyTextFilter), e => e.ReplyText.Contains(input.ReplyTextFilter))
                        .WhereIf(input.IsPublishedFilter.HasValue && input.IsPublishedFilter > -1, e => (input.IsPublishedFilter == 1 && e.IsPublished) || (input.IsPublishedFilter == 0 && !e.IsPublished))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreReviewReviewInfoFilter), e => e.StoreReviewFk != null && e.StoreReviewFk.ReviewInfo == input.StoreReviewReviewInfoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RatingLikeNameFilter), e => e.RatingLikeFk != null && e.RatingLikeFk.Name == input.RatingLikeNameFilter);

            var pagedAndFilteredStoreReviewFeedbacks = filteredStoreReviewFeedbacks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeReviewFeedbacks = from o in pagedAndFilteredStoreReviewFeedbacks
                                       join o1 in _lookup_storeReviewRepository.GetAll() on o.StoreReviewId equals o1.Id into j1
                                       from s1 in j1.DefaultIfEmpty()

                                       join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                                       from s2 in j2.DefaultIfEmpty()

                                       join o3 in _lookup_ratingLikeRepository.GetAll() on o.RatingLikeId equals o3.Id into j3
                                       from s3 in j3.DefaultIfEmpty()

                                       select new
                                       {

                                           o.ReplyText,
                                           o.IsPublished,
                                           Id = o.Id,
                                           StoreReviewReviewInfo = s1 == null || s1.ReviewInfo == null ? "" : s1.ReviewInfo.ToString(),
                                           ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString(),
                                           RatingLikeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                       };

            var totalCount = await filteredStoreReviewFeedbacks.CountAsync();

            var dbList = await storeReviewFeedbacks.ToListAsync();
            var results = new List<GetStoreReviewFeedbackForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreReviewFeedbackForViewDto()
                {
                    StoreReviewFeedback = new StoreReviewFeedbackDto
                    {

                        ReplyText = o.ReplyText,
                        IsPublished = o.IsPublished,
                        Id = o.Id,
                    },
                    StoreReviewReviewInfo = o.StoreReviewReviewInfo,
                    ContactFullName = o.ContactFullName,
                    RatingLikeName = o.RatingLikeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreReviewFeedbackForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreReviewFeedbackForViewDto> GetStoreReviewFeedbackForView(long id)
        {
            var storeReviewFeedback = await _storeReviewFeedbackRepository.GetAsync(id);

            var output = new GetStoreReviewFeedbackForViewDto { StoreReviewFeedback = ObjectMapper.Map<StoreReviewFeedbackDto>(storeReviewFeedback) };

            if (output.StoreReviewFeedback.StoreReviewId != null)
            {
                var _lookupStoreReview = await _lookup_storeReviewRepository.FirstOrDefaultAsync((long)output.StoreReviewFeedback.StoreReviewId);
                output.StoreReviewReviewInfo = _lookupStoreReview?.ReviewInfo?.ToString();
            }

            if (output.StoreReviewFeedback.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.StoreReviewFeedback.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.StoreReviewFeedback.RatingLikeId != null)
            {
                var _lookupRatingLike = await _lookup_ratingLikeRepository.FirstOrDefaultAsync((long)output.StoreReviewFeedback.RatingLikeId);
                output.RatingLikeName = _lookupRatingLike?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreReviewFeedbacks_Edit)]
        public async Task<GetStoreReviewFeedbackForEditOutput> GetStoreReviewFeedbackForEdit(EntityDto<long> input)
        {
            var storeReviewFeedback = await _storeReviewFeedbackRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreReviewFeedbackForEditOutput { StoreReviewFeedback = ObjectMapper.Map<CreateOrEditStoreReviewFeedbackDto>(storeReviewFeedback) };

            if (output.StoreReviewFeedback.StoreReviewId != null)
            {
                var _lookupStoreReview = await _lookup_storeReviewRepository.FirstOrDefaultAsync((long)output.StoreReviewFeedback.StoreReviewId);
                output.StoreReviewReviewInfo = _lookupStoreReview?.ReviewInfo?.ToString();
            }

            if (output.StoreReviewFeedback.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.StoreReviewFeedback.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.StoreReviewFeedback.RatingLikeId != null)
            {
                var _lookupRatingLike = await _lookup_ratingLikeRepository.FirstOrDefaultAsync((long)output.StoreReviewFeedback.RatingLikeId);
                output.RatingLikeName = _lookupRatingLike?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreReviewFeedbackDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreReviewFeedbacks_Create)]
        protected virtual async Task Create(CreateOrEditStoreReviewFeedbackDto input)
        {
            var storeReviewFeedback = ObjectMapper.Map<StoreReviewFeedback>(input);

            if (AbpSession.TenantId != null)
            {
                storeReviewFeedback.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeReviewFeedbackRepository.InsertAsync(storeReviewFeedback);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreReviewFeedbacks_Edit)]
        protected virtual async Task Update(CreateOrEditStoreReviewFeedbackDto input)
        {
            var storeReviewFeedback = await _storeReviewFeedbackRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeReviewFeedback);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreReviewFeedbacks_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeReviewFeedbackRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreReviewFeedbacksToExcel(GetAllStoreReviewFeedbacksForExcelInput input)
        {

            var filteredStoreReviewFeedbacks = _storeReviewFeedbackRepository.GetAll()
                        .Include(e => e.StoreReviewFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.RatingLikeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReplyText.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReplyTextFilter), e => e.ReplyText.Contains(input.ReplyTextFilter))
                        .WhereIf(input.IsPublishedFilter.HasValue && input.IsPublishedFilter > -1, e => (input.IsPublishedFilter == 1 && e.IsPublished) || (input.IsPublishedFilter == 0 && !e.IsPublished))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreReviewReviewInfoFilter), e => e.StoreReviewFk != null && e.StoreReviewFk.ReviewInfo == input.StoreReviewReviewInfoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RatingLikeNameFilter), e => e.RatingLikeFk != null && e.RatingLikeFk.Name == input.RatingLikeNameFilter);

            var query = (from o in filteredStoreReviewFeedbacks
                         join o1 in _lookup_storeReviewRepository.GetAll() on o.StoreReviewId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_ratingLikeRepository.GetAll() on o.RatingLikeId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetStoreReviewFeedbackForViewDto()
                         {
                             StoreReviewFeedback = new StoreReviewFeedbackDto
                             {
                                 ReplyText = o.ReplyText,
                                 IsPublished = o.IsPublished,
                                 Id = o.Id
                             },
                             StoreReviewReviewInfo = s1 == null || s1.ReviewInfo == null ? "" : s1.ReviewInfo.ToString(),
                             ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString(),
                             RatingLikeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var storeReviewFeedbackListDtos = await query.ToListAsync();

            return _storeReviewFeedbacksExcelExporter.ExportToFile(storeReviewFeedbackListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreReviewFeedbacks)]
        public async Task<PagedResultDto<StoreReviewFeedbackStoreReviewLookupTableDto>> GetAllStoreReviewForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeReviewRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.ReviewInfo != null && e.ReviewInfo.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeReviewList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreReviewFeedbackStoreReviewLookupTableDto>();
            foreach (var storeReview in storeReviewList)
            {
                lookupTableDtoList.Add(new StoreReviewFeedbackStoreReviewLookupTableDto
                {
                    Id = storeReview.Id,
                    DisplayName = storeReview.ReviewInfo?.ToString()
                });
            }

            return new PagedResultDto<StoreReviewFeedbackStoreReviewLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreReviewFeedbacks)]
        public async Task<PagedResultDto<StoreReviewFeedbackContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreReviewFeedbackContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new StoreReviewFeedbackContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<StoreReviewFeedbackContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreReviewFeedbacks)]
        public async Task<PagedResultDto<StoreReviewFeedbackRatingLikeLookupTableDto>> GetAllRatingLikeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_ratingLikeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var ratingLikeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreReviewFeedbackRatingLikeLookupTableDto>();
            foreach (var ratingLike in ratingLikeList)
            {
                lookupTableDtoList.Add(new StoreReviewFeedbackRatingLikeLookupTableDto
                {
                    Id = ratingLike.Id,
                    DisplayName = ratingLike.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreReviewFeedbackRatingLikeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}