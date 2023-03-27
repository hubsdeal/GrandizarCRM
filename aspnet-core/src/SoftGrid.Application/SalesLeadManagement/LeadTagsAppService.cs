using SoftGrid.SalesLeadManagement;
using SoftGrid.LookupData;
using SoftGrid.LookupData;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.SalesLeadManagement.Exporting;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.SalesLeadManagement
{
    [AbpAuthorize(AppPermissions.Pages_LeadTags)]
    public class LeadTagsAppService : SoftGridAppServiceBase, ILeadTagsAppService
    {
        private readonly IRepository<LeadTag, long> _leadTagRepository;
        private readonly ILeadTagsExcelExporter _leadTagsExcelExporter;
        private readonly IRepository<Lead, long> _lookup_leadRepository;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;
        private readonly IRepository<MasterTag, long> _lookup_masterTagRepository;

        public LeadTagsAppService(IRepository<LeadTag, long> leadTagRepository, ILeadTagsExcelExporter leadTagsExcelExporter, IRepository<Lead, long> lookup_leadRepository, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository, IRepository<MasterTag, long> lookup_masterTagRepository)
        {
            _leadTagRepository = leadTagRepository;
            _leadTagsExcelExporter = leadTagsExcelExporter;
            _lookup_leadRepository = lookup_leadRepository;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;
            _lookup_masterTagRepository = lookup_masterTagRepository;

        }

        public async Task<PagedResultDto<GetLeadTagForViewDto>> GetAll(GetAllLeadTagsInput input)
        {

            var filteredLeadTags = _leadTagRepository.GetAll()
                        .Include(e => e.LeadFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTag.Contains(input.Filter) || e.TagValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagFilter), e => e.CustomTag.Contains(input.CustomTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagValueFilter), e => e.TagValue.Contains(input.TagValueFilter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadTitleFilter), e => e.LeadFk != null && e.LeadFk.Title == input.LeadTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var pagedAndFilteredLeadTags = filteredLeadTags
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var leadTags = from o in pagedAndFilteredLeadTags
                           join o1 in _lookup_leadRepository.GetAll() on o.LeadId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           join o3 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o3.Id into j3
                           from s3 in j3.DefaultIfEmpty()

                           select new
                           {

                               o.CustomTag,
                               o.TagValue,
                               o.DisplaySequence,
                               Id = o.Id,
                               LeadTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                               MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                               MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                           };

            var totalCount = await filteredLeadTags.CountAsync();

            var dbList = await leadTags.ToListAsync();
            var results = new List<GetLeadTagForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLeadTagForViewDto()
                {
                    LeadTag = new LeadTagDto
                    {

                        CustomTag = o.CustomTag,
                        TagValue = o.TagValue,
                        DisplaySequence = o.DisplaySequence,
                        Id = o.Id,
                    },
                    LeadTitle = o.LeadTitle,
                    MasterTagCategoryName = o.MasterTagCategoryName,
                    MasterTagName = o.MasterTagName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLeadTagForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetLeadTagForViewDto> GetLeadTagForView(long id)
        {
            var leadTag = await _leadTagRepository.GetAsync(id);

            var output = new GetLeadTagForViewDto { LeadTag = ObjectMapper.Map<LeadTagDto>(leadTag) };

            if (output.LeadTag.LeadId != null)
            {
                var _lookupLead = await _lookup_leadRepository.FirstOrDefaultAsync((long)output.LeadTag.LeadId);
                output.LeadTitle = _lookupLead?.Title?.ToString();
            }

            if (output.LeadTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.LeadTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.LeadTag.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.LeadTag.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_LeadTags_Edit)]
        public async Task<GetLeadTagForEditOutput> GetLeadTagForEdit(EntityDto<long> input)
        {
            var leadTag = await _leadTagRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLeadTagForEditOutput { LeadTag = ObjectMapper.Map<CreateOrEditLeadTagDto>(leadTag) };

            if (output.LeadTag.LeadId != null)
            {
                var _lookupLead = await _lookup_leadRepository.FirstOrDefaultAsync((long)output.LeadTag.LeadId);
                output.LeadTitle = _lookupLead?.Title?.ToString();
            }

            if (output.LeadTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.LeadTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.LeadTag.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.LeadTag.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLeadTagDto input)
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

        [AbpAuthorize(AppPermissions.Pages_LeadTags_Create)]
        protected virtual async Task Create(CreateOrEditLeadTagDto input)
        {
            var leadTag = ObjectMapper.Map<LeadTag>(input);

            if (AbpSession.TenantId != null)
            {
                leadTag.TenantId = (int?)AbpSession.TenantId;
            }

            await _leadTagRepository.InsertAsync(leadTag);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadTags_Edit)]
        protected virtual async Task Update(CreateOrEditLeadTagDto input)
        {
            var leadTag = await _leadTagRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, leadTag);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadTags_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _leadTagRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetLeadTagsToExcel(GetAllLeadTagsForExcelInput input)
        {

            var filteredLeadTags = _leadTagRepository.GetAll()
                        .Include(e => e.LeadFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTag.Contains(input.Filter) || e.TagValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagFilter), e => e.CustomTag.Contains(input.CustomTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagValueFilter), e => e.TagValue.Contains(input.TagValueFilter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadTitleFilter), e => e.LeadFk != null && e.LeadFk.Title == input.LeadTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var query = (from o in filteredLeadTags
                         join o1 in _lookup_leadRepository.GetAll() on o.LeadId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetLeadTagForViewDto()
                         {
                             LeadTag = new LeadTagDto
                             {
                                 CustomTag = o.CustomTag,
                                 TagValue = o.TagValue,
                                 DisplaySequence = o.DisplaySequence,
                                 Id = o.Id
                             },
                             LeadTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                             MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var leadTagListDtos = await query.ToListAsync();

            return _leadTagsExcelExporter.ExportToFile(leadTagListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_LeadTags)]
        public async Task<PagedResultDto<LeadTagLeadLookupTableDto>> GetAllLeadForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_leadRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Title != null && e.Title.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var leadList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadTagLeadLookupTableDto>();
            foreach (var lead in leadList)
            {
                lookupTableDtoList.Add(new LeadTagLeadLookupTableDto
                {
                    Id = lead.Id,
                    DisplayName = lead.Title?.ToString()
                });
            }

            return new PagedResultDto<LeadTagLeadLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_LeadTags)]
        public async Task<PagedResultDto<LeadTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadTagMasterTagCategoryLookupTableDto>();
            foreach (var masterTagCategory in masterTagCategoryList)
            {
                lookupTableDtoList.Add(new LeadTagMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<LeadTagMasterTagCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_LeadTags)]
        public async Task<PagedResultDto<LeadTagMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadTagMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new LeadTagMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<LeadTagMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}