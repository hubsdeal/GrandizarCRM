using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.LookupData.Exporting;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.LookupData
{
    [AbpAuthorize(AppPermissions.Pages_RatingLikes)]
    public class RatingLikesAppService : SoftGridAppServiceBase, IRatingLikesAppService
    {
        private readonly IRepository<RatingLike, long> _ratingLikeRepository;
        private readonly IRatingLikesExcelExporter _ratingLikesExcelExporter;

        public RatingLikesAppService(IRepository<RatingLike, long> ratingLikeRepository, IRatingLikesExcelExporter ratingLikesExcelExporter)
        {
            _ratingLikeRepository = ratingLikeRepository;
            _ratingLikesExcelExporter = ratingLikesExcelExporter;

        }

        public async Task<PagedResultDto<GetRatingLikeForViewDto>> GetAll(GetAllRatingLikesInput input)
        {

            var filteredRatingLikes = _ratingLikeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.IconLink.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinScoreFilter != null, e => e.Score >= input.MinScoreFilter)
                        .WhereIf(input.MaxScoreFilter != null, e => e.Score <= input.MaxScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IconLinkFilter), e => e.IconLink.Contains(input.IconLinkFilter));

            var pagedAndFilteredRatingLikes = filteredRatingLikes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var ratingLikes = from o in pagedAndFilteredRatingLikes
                              select new
                              {

                                  o.Name,
                                  o.Score,
                                  o.IconLink,
                                  Id = o.Id
                              };

            var totalCount = await filteredRatingLikes.CountAsync();

            var dbList = await ratingLikes.ToListAsync();
            var results = new List<GetRatingLikeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRatingLikeForViewDto()
                {
                    RatingLike = new RatingLikeDto
                    {

                        Name = o.Name,
                        Score = o.Score,
                        IconLink = o.IconLink,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetRatingLikeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetRatingLikeForViewDto> GetRatingLikeForView(long id)
        {
            var ratingLike = await _ratingLikeRepository.GetAsync(id);

            var output = new GetRatingLikeForViewDto { RatingLike = ObjectMapper.Map<RatingLikeDto>(ratingLike) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_RatingLikes_Edit)]
        public async Task<GetRatingLikeForEditOutput> GetRatingLikeForEdit(EntityDto<long> input)
        {
            var ratingLike = await _ratingLikeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRatingLikeForEditOutput { RatingLike = ObjectMapper.Map<CreateOrEditRatingLikeDto>(ratingLike) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRatingLikeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_RatingLikes_Create)]
        protected virtual async Task Create(CreateOrEditRatingLikeDto input)
        {
            var ratingLike = ObjectMapper.Map<RatingLike>(input);

            if (AbpSession.TenantId != null)
            {
                ratingLike.TenantId = (int?)AbpSession.TenantId;
            }

            await _ratingLikeRepository.InsertAsync(ratingLike);

        }

        [AbpAuthorize(AppPermissions.Pages_RatingLikes_Edit)]
        protected virtual async Task Update(CreateOrEditRatingLikeDto input)
        {
            var ratingLike = await _ratingLikeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, ratingLike);

        }

        [AbpAuthorize(AppPermissions.Pages_RatingLikes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _ratingLikeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRatingLikesToExcel(GetAllRatingLikesForExcelInput input)
        {

            var filteredRatingLikes = _ratingLikeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.IconLink.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinScoreFilter != null, e => e.Score >= input.MinScoreFilter)
                        .WhereIf(input.MaxScoreFilter != null, e => e.Score <= input.MaxScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IconLinkFilter), e => e.IconLink.Contains(input.IconLinkFilter));

            var query = (from o in filteredRatingLikes
                         select new GetRatingLikeForViewDto()
                         {
                             RatingLike = new RatingLikeDto
                             {
                                 Name = o.Name,
                                 Score = o.Score,
                                 IconLink = o.IconLink,
                                 Id = o.Id
                             }
                         });

            var ratingLikeListDtos = await query.ToListAsync();

            return _ratingLikesExcelExporter.ExportToFile(ratingLikeListDtos);
        }

    }
}