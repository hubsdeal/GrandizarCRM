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
    [AbpAuthorize(AppPermissions.Pages_MasterTags)]
    public class MasterTagsAppService : SoftGridAppServiceBase, IMasterTagsAppService
    {
        private readonly IRepository<MasterTag, long> _masterTagRepository;
        private readonly IMasterTagsExcelExporter _masterTagsExcelExporter;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;
        private readonly IRepository<MediaLibrary, long> _lookup_mediaLibraryRepository;

        public MasterTagsAppService(IRepository<MasterTag, long> masterTagRepository, IMasterTagsExcelExporter masterTagsExcelExporter, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository, IRepository<MediaLibrary, long> lookup_mediaLibraryRepository)
        {
            _masterTagRepository = masterTagRepository;
            _masterTagsExcelExporter = masterTagsExcelExporter;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;
            _lookup_mediaLibraryRepository = lookup_mediaLibraryRepository;

        }

        public async Task<PagedResultDto<GetMasterTagForViewDto>> GetAll(GetAllMasterTagsInput input)
        {

            var filteredMasterTags = _masterTagRepository.GetAll()
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.PictureMediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Synonyms.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SynonymsFilter), e => e.Synonyms.Contains(input.SynonymsFilter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.PictureMediaLibraryFk != null && e.PictureMediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var pagedAndFilteredMasterTags = filteredMasterTags
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var masterTags = from o in pagedAndFilteredMasterTags
                             join o1 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _lookup_mediaLibraryRepository.GetAll() on o.PictureMediaLibraryId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             select new
                             {

                                 o.Name,
                                 o.Description,
                                 o.Synonyms,
                                 o.DisplaySequence,
                                 Id = o.Id,
                                 MasterTagCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                 MediaLibraryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                             };

            var totalCount = await filteredMasterTags.CountAsync();

            var dbList = await masterTags.ToListAsync();
            var results = new List<GetMasterTagForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetMasterTagForViewDto()
                {
                    MasterTag = new MasterTagDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Synonyms = o.Synonyms,
                        DisplaySequence = o.DisplaySequence,
                        Id = o.Id,
                    },
                    MasterTagCategoryName = o.MasterTagCategoryName,
                    MediaLibraryName = o.MediaLibraryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetMasterTagForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetMasterTagForViewDto> GetMasterTagForView(long id)
        {
            var masterTag = await _masterTagRepository.GetAsync(id);

            var output = new GetMasterTagForViewDto { MasterTag = ObjectMapper.Map<MasterTagDto>(masterTag) };

            if (output.MasterTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.MasterTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.MasterTag.PictureMediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.MasterTag.PictureMediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MasterTags_Edit)]
        public async Task<GetMasterTagForEditOutput> GetMasterTagForEdit(EntityDto<long> input)
        {
            var masterTag = await _masterTagRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMasterTagForEditOutput { MasterTag = ObjectMapper.Map<CreateOrEditMasterTagDto>(masterTag) };

            if (output.MasterTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.MasterTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.MasterTag.PictureMediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.MasterTag.PictureMediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMasterTagDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MasterTags_Create)]
        protected virtual async Task Create(CreateOrEditMasterTagDto input)
        {
            var masterTag = ObjectMapper.Map<MasterTag>(input);

            if (AbpSession.TenantId != null)
            {
                masterTag.TenantId = (int?)AbpSession.TenantId;
            }

            await _masterTagRepository.InsertAsync(masterTag);

        }

        [AbpAuthorize(AppPermissions.Pages_MasterTags_Edit)]
        protected virtual async Task Update(CreateOrEditMasterTagDto input)
        {
            var masterTag = await _masterTagRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, masterTag);

        }

        [AbpAuthorize(AppPermissions.Pages_MasterTags_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _masterTagRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMasterTagsToExcel(GetAllMasterTagsForExcelInput input)
        {

            var filteredMasterTags = _masterTagRepository.GetAll()
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.PictureMediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Synonyms.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SynonymsFilter), e => e.Synonyms.Contains(input.SynonymsFilter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.PictureMediaLibraryFk != null && e.PictureMediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var query = (from o in filteredMasterTags
                         join o1 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_mediaLibraryRepository.GetAll() on o.PictureMediaLibraryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetMasterTagForViewDto()
                         {
                             MasterTag = new MasterTagDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Synonyms = o.Synonyms,
                                 DisplaySequence = o.DisplaySequence,
                                 Id = o.Id
                             },
                             MasterTagCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MediaLibraryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var masterTagListDtos = await query.ToListAsync();

            return _masterTagsExcelExporter.ExportToFile(masterTagListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_MasterTags)]
        public async Task<List<MasterTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForTableDropdown()
        {
            return await _lookup_masterTagCategoryRepository.GetAll()
                .Select(masterTagCategory => new MasterTagMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory == null || masterTagCategory.Name == null ? "" : masterTagCategory.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_MasterTags)]
        public async Task<PagedResultDto<MasterTagMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_mediaLibraryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var mediaLibraryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<MasterTagMediaLibraryLookupTableDto>();
            foreach (var mediaLibrary in mediaLibraryList)
            {
                lookupTableDtoList.Add(new MasterTagMediaLibraryLookupTableDto
                {
                    Id = mediaLibrary.Id,
                    DisplayName = mediaLibrary.Name?.ToString()
                });
            }

            return new PagedResultDto<MasterTagMediaLibraryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}