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

        public MasterTagsAppService(IRepository<MasterTag, long> masterTagRepository, IMasterTagsExcelExporter masterTagsExcelExporter, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository)
        {
            _masterTagRepository = masterTagRepository;
            _masterTagsExcelExporter = masterTagsExcelExporter;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;

        }

        public async Task<PagedResultDto<GetMasterTagForViewDto>> GetAll(GetAllMasterTagsInput input)
        {

            var filteredMasterTags = _masterTagRepository.GetAll()
                        .Include(e => e.MasterTagCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Synonyms.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SynonymsFilter), e => e.Synonyms.Contains(input.SynonymsFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PictureIdFilter.ToString()), e => e.PictureId.ToString() == input.PictureIdFilter.ToString())
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter);

            var pagedAndFilteredMasterTags = filteredMasterTags
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var masterTags = from o in pagedAndFilteredMasterTags
                             join o1 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             select new
                             {

                                 o.Name,
                                 o.Description,
                                 o.Synonyms,
                                 o.PictureId,
                                 o.DisplaySequence,
                                 Id = o.Id,
                                 MasterTagCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
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
                        PictureId = o.PictureId,
                        DisplaySequence = o.DisplaySequence,
                        Id = o.Id,
                    },
                    MasterTagCategoryName = o.MasterTagCategoryName
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
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Synonyms.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SynonymsFilter), e => e.Synonyms.Contains(input.SynonymsFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PictureIdFilter.ToString()), e => e.PictureId.ToString() == input.PictureIdFilter.ToString())
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter);

            var query = (from o in filteredMasterTags
                         join o1 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetMasterTagForViewDto()
                         {
                             MasterTag = new MasterTagDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Synonyms = o.Synonyms,
                                 PictureId = o.PictureId,
                                 DisplaySequence = o.DisplaySequence,
                                 Id = o.Id
                             },
                             MasterTagCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
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

    }
}