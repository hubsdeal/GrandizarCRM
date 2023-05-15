using SoftGrid.LookupData;

using SoftGrid.CMS.Enums;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.CMS.Exporting;
using SoftGrid.CMS.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.CMS
{
    [AbpAuthorize(AppPermissions.Pages_Contents)]
    public class ContentsAppService : SoftGridAppServiceBase, IContentsAppService
    {
        private readonly IRepository<Content, long> _contentRepository;
        private readonly IContentsExcelExporter _contentsExcelExporter;
        private readonly IRepository<MediaLibrary, long> _lookup_mediaLibraryRepository;

        public ContentsAppService(IRepository<Content, long> contentRepository, IContentsExcelExporter contentsExcelExporter, IRepository<MediaLibrary, long> lookup_mediaLibraryRepository)
        {
            _contentRepository = contentRepository;
            _contentsExcelExporter = contentsExcelExporter;
            _lookup_mediaLibraryRepository = lookup_mediaLibraryRepository;

        }

        public async Task<PagedResultDto<GetContentForViewDto>> GetAll(GetAllContentsInput input)
        {
            var contentTypeIdFilter = input.ContentTypeIdFilter.HasValue
                        ? (ContentType)input.ContentTypeIdFilter
                        : default;

            var filteredContents = _contentRepository.GetAll()
                        .Include(e => e.BannerMediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Body.Contains(input.Filter) || e.PublishTime.Contains(input.Filter) || e.SeoUrl.Contains(input.Filter) || e.SeoKeywords.Contains(input.Filter) || e.SeoDescription.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title.Contains(input.TitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BodyFilter), e => e.Body.Contains(input.BodyFilter))
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(input.MinPublishedDateFilter != null, e => e.PublishedDate >= input.MinPublishedDateFilter)
                        .WhereIf(input.MaxPublishedDateFilter != null, e => e.PublishedDate <= input.MaxPublishedDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PublishTimeFilter), e => e.PublishTime.Contains(input.PublishTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SeoUrlFilter), e => e.SeoUrl.Contains(input.SeoUrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SeoKeywordsFilter), e => e.SeoKeywords.Contains(input.SeoKeywordsFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SeoDescriptionFilter), e => e.SeoDescription.Contains(input.SeoDescriptionFilter))
                        .WhereIf(input.ContentTypeIdFilter.HasValue && input.ContentTypeIdFilter > -1, e => e.ContentTypeId == contentTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.BannerMediaLibraryFk != null && e.BannerMediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var pagedAndFilteredContents = filteredContents
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var contents = from o in pagedAndFilteredContents
                           join o1 in _lookup_mediaLibraryRepository.GetAll() on o.BannerMediaLibraryId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           select new
                           {

                               o.Title,
                               o.Body,
                               o.Published,
                               o.PublishedDate,
                               o.PublishTime,
                               o.SeoUrl,
                               o.SeoKeywords,
                               o.SeoDescription,
                               o.ContentTypeId,
                               Id = o.Id,
                               MediaLibraryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                           };

            var totalCount = await filteredContents.CountAsync();

            var dbList = await contents.ToListAsync();
            var results = new List<GetContentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetContentForViewDto()
                {
                    Content = new ContentDto
                    {

                        Title = o.Title,
                        Body = o.Body,
                        Published = o.Published,
                        PublishedDate = o.PublishedDate,
                        PublishTime = o.PublishTime,
                        SeoUrl = o.SeoUrl,
                        SeoKeywords = o.SeoKeywords,
                        SeoDescription = o.SeoDescription,
                        ContentTypeId = o.ContentTypeId,
                        Id = o.Id,
                    },
                    MediaLibraryName = o.MediaLibraryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetContentForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetContentForViewDto> GetContentForView(long id)
        {
            var content = await _contentRepository.GetAsync(id);

            var output = new GetContentForViewDto { Content = ObjectMapper.Map<ContentDto>(content) };

            if (output.Content.BannerMediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.Content.BannerMediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Contents_Edit)]
        public async Task<GetContentForEditOutput> GetContentForEdit(EntityDto<long> input)
        {
            var content = await _contentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetContentForEditOutput { Content = ObjectMapper.Map<CreateOrEditContentDto>(content) };

            if (output.Content.BannerMediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.Content.BannerMediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditContentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Contents_Create)]
        protected virtual async Task Create(CreateOrEditContentDto input)
        {
            var content = ObjectMapper.Map<Content>(input);

            if (AbpSession.TenantId != null)
            {
                content.TenantId = (int?)AbpSession.TenantId;
            }

            await _contentRepository.InsertAsync(content);

        }

        [AbpAuthorize(AppPermissions.Pages_Contents_Edit)]
        protected virtual async Task Update(CreateOrEditContentDto input)
        {
            var content = await _contentRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, content);

        }

        [AbpAuthorize(AppPermissions.Pages_Contents_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _contentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetContentsToExcel(GetAllContentsForExcelInput input)
        {
            var contentTypeIdFilter = input.ContentTypeIdFilter.HasValue
                        ? (ContentType)input.ContentTypeIdFilter
                        : default;

            var filteredContents = _contentRepository.GetAll()
                        .Include(e => e.BannerMediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Body.Contains(input.Filter) || e.PublishTime.Contains(input.Filter) || e.SeoUrl.Contains(input.Filter) || e.SeoKeywords.Contains(input.Filter) || e.SeoDescription.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title.Contains(input.TitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BodyFilter), e => e.Body.Contains(input.BodyFilter))
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(input.MinPublishedDateFilter != null, e => e.PublishedDate >= input.MinPublishedDateFilter)
                        .WhereIf(input.MaxPublishedDateFilter != null, e => e.PublishedDate <= input.MaxPublishedDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PublishTimeFilter), e => e.PublishTime.Contains(input.PublishTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SeoUrlFilter), e => e.SeoUrl.Contains(input.SeoUrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SeoKeywordsFilter), e => e.SeoKeywords.Contains(input.SeoKeywordsFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SeoDescriptionFilter), e => e.SeoDescription.Contains(input.SeoDescriptionFilter))
                        .WhereIf(input.ContentTypeIdFilter.HasValue && input.ContentTypeIdFilter > -1, e => e.ContentTypeId == contentTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.BannerMediaLibraryFk != null && e.BannerMediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var query = (from o in filteredContents
                         join o1 in _lookup_mediaLibraryRepository.GetAll() on o.BannerMediaLibraryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetContentForViewDto()
                         {
                             Content = new ContentDto
                             {
                                 Title = o.Title,
                                 Body = o.Body,
                                 Published = o.Published,
                                 PublishedDate = o.PublishedDate,
                                 PublishTime = o.PublishTime,
                                 SeoUrl = o.SeoUrl,
                                 SeoKeywords = o.SeoKeywords,
                                 SeoDescription = o.SeoDescription,
                                 ContentTypeId = o.ContentTypeId,
                                 Id = o.Id
                             },
                             MediaLibraryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var contentListDtos = await query.ToListAsync();

            return _contentsExcelExporter.ExportToFile(contentListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Contents)]
        public async Task<PagedResultDto<ContentMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_mediaLibraryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var mediaLibraryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ContentMediaLibraryLookupTableDto>();
            foreach (var mediaLibrary in mediaLibraryList)
            {
                lookupTableDtoList.Add(new ContentMediaLibraryLookupTableDto
                {
                    Id = mediaLibrary.Id,
                    DisplayName = mediaLibrary.Name?.ToString()
                });
            }

            return new PagedResultDto<ContentMediaLibraryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}