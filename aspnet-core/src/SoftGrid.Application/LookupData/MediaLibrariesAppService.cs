using SoftGrid.LookupData;
using SoftGrid.LookupData;

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
    [AbpAuthorize(AppPermissions.Pages_MediaLibraries)]
    public class MediaLibrariesAppService : SoftGridAppServiceBase, IMediaLibrariesAppService
    {
        private readonly IRepository<MediaLibrary, long> _mediaLibraryRepository;
        private readonly IMediaLibrariesExcelExporter _mediaLibrariesExcelExporter;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;
        private readonly IRepository<MasterTag, long> _lookup_masterTagRepository;

        public MediaLibrariesAppService(IRepository<MediaLibrary, long> mediaLibraryRepository, IMediaLibrariesExcelExporter mediaLibrariesExcelExporter, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository, IRepository<MasterTag, long> lookup_masterTagRepository)
        {
            _mediaLibraryRepository = mediaLibraryRepository;
            _mediaLibrariesExcelExporter = mediaLibrariesExcelExporter;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;
            _lookup_masterTagRepository = lookup_masterTagRepository;

        }

        public async Task<PagedResultDto<GetMediaLibraryForViewDto>> GetAll(GetAllMediaLibrariesInput input)
        {

            var filteredMediaLibraries = _mediaLibraryRepository.GetAll()
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Size.Contains(input.Filter) || e.FileExtension.Contains(input.Filter) || e.Dimension.Contains(input.Filter) || e.VideoLink.Contains(input.Filter) || e.SeoTag.Contains(input.Filter) || e.AltTag.Contains(input.Filter) || e.VirtualPath.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SizeFilter), e => e.Size.Contains(input.SizeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileExtensionFilter), e => e.FileExtension.Contains(input.FileExtensionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DimensionFilter), e => e.Dimension.Contains(input.DimensionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VideoLinkFilter), e => e.VideoLink.Contains(input.VideoLinkFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SeoTagFilter), e => e.SeoTag.Contains(input.SeoTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AltTagFilter), e => e.AltTag.Contains(input.AltTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VirtualPathFilter), e => e.VirtualPath.Contains(input.VirtualPathFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BinaryObjectIdFilter.ToString()), e => e.BinaryObjectId.ToString() == input.BinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var pagedAndFilteredMediaLibraries = filteredMediaLibraries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var mediaLibraries = from o in pagedAndFilteredMediaLibraries
                                 join o1 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 join o2 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()

                                 select new
                                 {

                                     o.Name,
                                     o.Size,
                                     o.FileExtension,
                                     o.Dimension,
                                     o.VideoLink,
                                     o.SeoTag,
                                     o.AltTag,
                                     o.VirtualPath,
                                     o.BinaryObjectId,
                                     Id = o.Id,
                                     MasterTagCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                     MasterTagName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                 };

            var totalCount = await filteredMediaLibraries.CountAsync();

            var dbList = await mediaLibraries.ToListAsync();
            var results = new List<GetMediaLibraryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetMediaLibraryForViewDto()
                {
                    MediaLibrary = new MediaLibraryDto
                    {

                        Name = o.Name,
                        Size = o.Size,
                        FileExtension = o.FileExtension,
                        Dimension = o.Dimension,
                        VideoLink = o.VideoLink,
                        SeoTag = o.SeoTag,
                        AltTag = o.AltTag,
                        VirtualPath = o.VirtualPath,
                        BinaryObjectId = o.BinaryObjectId,
                        Id = o.Id,
                    },
                    MasterTagCategoryName = o.MasterTagCategoryName,
                    MasterTagName = o.MasterTagName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetMediaLibraryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetMediaLibraryForViewDto> GetMediaLibraryForView(long id)
        {
            var mediaLibrary = await _mediaLibraryRepository.GetAsync(id);

            var output = new GetMediaLibraryForViewDto { MediaLibrary = ObjectMapper.Map<MediaLibraryDto>(mediaLibrary) };

            if (output.MediaLibrary.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.MediaLibrary.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.MediaLibrary.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.MediaLibrary.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MediaLibraries_Edit)]
        public async Task<GetMediaLibraryForEditOutput> GetMediaLibraryForEdit(EntityDto<long> input)
        {
            var mediaLibrary = await _mediaLibraryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMediaLibraryForEditOutput { MediaLibrary = ObjectMapper.Map<CreateOrEditMediaLibraryDto>(mediaLibrary) };

            if (output.MediaLibrary.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.MediaLibrary.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.MediaLibrary.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.MediaLibrary.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMediaLibraryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MediaLibraries_Create)]
        protected virtual async Task Create(CreateOrEditMediaLibraryDto input)
        {
            var mediaLibrary = ObjectMapper.Map<MediaLibrary>(input);

            if (AbpSession.TenantId != null)
            {
                mediaLibrary.TenantId = (int?)AbpSession.TenantId;
            }

            await _mediaLibraryRepository.InsertAsync(mediaLibrary);

        }

        [AbpAuthorize(AppPermissions.Pages_MediaLibraries_Edit)]
        protected virtual async Task Update(CreateOrEditMediaLibraryDto input)
        {
            var mediaLibrary = await _mediaLibraryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, mediaLibrary);

        }

        [AbpAuthorize(AppPermissions.Pages_MediaLibraries_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _mediaLibraryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMediaLibrariesToExcel(GetAllMediaLibrariesForExcelInput input)
        {

            var filteredMediaLibraries = _mediaLibraryRepository.GetAll()
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Size.Contains(input.Filter) || e.FileExtension.Contains(input.Filter) || e.Dimension.Contains(input.Filter) || e.VideoLink.Contains(input.Filter) || e.SeoTag.Contains(input.Filter) || e.AltTag.Contains(input.Filter) || e.VirtualPath.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SizeFilter), e => e.Size.Contains(input.SizeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileExtensionFilter), e => e.FileExtension.Contains(input.FileExtensionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DimensionFilter), e => e.Dimension.Contains(input.DimensionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VideoLinkFilter), e => e.VideoLink.Contains(input.VideoLinkFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SeoTagFilter), e => e.SeoTag.Contains(input.SeoTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AltTagFilter), e => e.AltTag.Contains(input.AltTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VirtualPathFilter), e => e.VirtualPath.Contains(input.VirtualPathFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BinaryObjectIdFilter.ToString()), e => e.BinaryObjectId.ToString() == input.BinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var query = (from o in filteredMediaLibraries
                         join o1 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetMediaLibraryForViewDto()
                         {
                             MediaLibrary = new MediaLibraryDto
                             {
                                 Name = o.Name,
                                 Size = o.Size,
                                 FileExtension = o.FileExtension,
                                 Dimension = o.Dimension,
                                 VideoLink = o.VideoLink,
                                 SeoTag = o.SeoTag,
                                 AltTag = o.AltTag,
                                 VirtualPath = o.VirtualPath,
                                 BinaryObjectId = o.BinaryObjectId,
                                 Id = o.Id
                             },
                             MasterTagCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MasterTagName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var mediaLibraryListDtos = await query.ToListAsync();

            return _mediaLibrariesExcelExporter.ExportToFile(mediaLibraryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_MediaLibraries)]
        public async Task<PagedResultDto<MediaLibraryMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<MediaLibraryMasterTagCategoryLookupTableDto>();
            foreach (var masterTagCategory in masterTagCategoryList)
            {
                lookupTableDtoList.Add(new MediaLibraryMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<MediaLibraryMasterTagCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_MediaLibraries)]
        public async Task<PagedResultDto<MediaLibraryMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<MediaLibraryMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new MediaLibraryMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<MediaLibraryMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}