using SoftGrid.JobManagement;
using SoftGrid.LookupData;
using SoftGrid.LookupData;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.JobManagement.Exporting;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.JobManagement
{
    [AbpAuthorize(AppPermissions.Pages_JobTags)]
    public class JobTagsAppService : SoftGridAppServiceBase, IJobTagsAppService
    {
        private readonly IRepository<JobTag, long> _jobTagRepository;
        private readonly IJobTagsExcelExporter _jobTagsExcelExporter;
        private readonly IRepository<Job, long> _lookup_jobRepository;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;
        private readonly IRepository<MasterTag, long> _lookup_masterTagRepository;

        public JobTagsAppService(IRepository<JobTag, long> jobTagRepository, IJobTagsExcelExporter jobTagsExcelExporter, IRepository<Job, long> lookup_jobRepository, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository, IRepository<MasterTag, long> lookup_masterTagRepository)
        {
            _jobTagRepository = jobTagRepository;
            _jobTagsExcelExporter = jobTagsExcelExporter;
            _lookup_jobRepository = lookup_jobRepository;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;
            _lookup_masterTagRepository = lookup_masterTagRepository;

        }

        public async Task<PagedResultDto<GetJobTagForViewDto>> GetAll(GetAllJobTagsInput input)
        {

            var filteredJobTags = _jobTagRepository.GetAll()
                        .Include(e => e.JobFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTag.Contains(input.Filter) || e.TagValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagFilter), e => e.CustomTag.Contains(input.CustomTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagValueFilter), e => e.TagValue.Contains(input.TagValueFilter))
                        .WhereIf(input.VerifiedFilter.HasValue && input.VerifiedFilter > -1, e => (input.VerifiedFilter == 1 && e.Verified) || (input.VerifiedFilter == 0 && !e.Verified))
                        .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                        .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobFk != null && e.JobFk.Title == input.JobTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var pagedAndFilteredJobTags = filteredJobTags
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var jobTags = from o in pagedAndFilteredJobTags
                          join o1 in _lookup_jobRepository.GetAll() on o.JobId equals o1.Id into j1
                          from s1 in j1.DefaultIfEmpty()

                          join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                          from s2 in j2.DefaultIfEmpty()

                          join o3 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o3.Id into j3
                          from s3 in j3.DefaultIfEmpty()

                          select new
                          {

                              o.CustomTag,
                              o.TagValue,
                              o.Verified,
                              o.Sequence,
                              Id = o.Id,
                              JobTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                              MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                              MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                          };

            var totalCount = await filteredJobTags.CountAsync();

            var dbList = await jobTags.ToListAsync();
            var results = new List<GetJobTagForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetJobTagForViewDto()
                {
                    JobTag = new JobTagDto
                    {

                        CustomTag = o.CustomTag,
                        TagValue = o.TagValue,
                        Verified = o.Verified,
                        Sequence = o.Sequence,
                        Id = o.Id,
                    },
                    JobTitle = o.JobTitle,
                    MasterTagCategoryName = o.MasterTagCategoryName,
                    MasterTagName = o.MasterTagName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetJobTagForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetJobTagForViewDto> GetJobTagForView(long id)
        {
            var jobTag = await _jobTagRepository.GetAsync(id);

            var output = new GetJobTagForViewDto { JobTag = ObjectMapper.Map<JobTagDto>(jobTag) };

            if (output.JobTag.JobId != null)
            {
                var _lookupJob = await _lookup_jobRepository.FirstOrDefaultAsync((long)output.JobTag.JobId);
                output.JobTitle = _lookupJob?.Title?.ToString();
            }

            if (output.JobTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.JobTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.JobTag.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.JobTag.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_JobTags_Edit)]
        public async Task<GetJobTagForEditOutput> GetJobTagForEdit(EntityDto<long> input)
        {
            var jobTag = await _jobTagRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetJobTagForEditOutput { JobTag = ObjectMapper.Map<CreateOrEditJobTagDto>(jobTag) };

            if (output.JobTag.JobId != null)
            {
                var _lookupJob = await _lookup_jobRepository.FirstOrDefaultAsync((long)output.JobTag.JobId);
                output.JobTitle = _lookupJob?.Title?.ToString();
            }

            if (output.JobTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.JobTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.JobTag.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.JobTag.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditJobTagDto input)
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

        [AbpAuthorize(AppPermissions.Pages_JobTags_Create)]
        protected virtual async Task Create(CreateOrEditJobTagDto input)
        {
            var jobTag = ObjectMapper.Map<JobTag>(input);

            if (AbpSession.TenantId != null)
            {
                jobTag.TenantId = (int?)AbpSession.TenantId;
            }

            await _jobTagRepository.InsertAsync(jobTag);

        }

        [AbpAuthorize(AppPermissions.Pages_JobTags_Edit)]
        protected virtual async Task Update(CreateOrEditJobTagDto input)
        {
            var jobTag = await _jobTagRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, jobTag);

        }

        [AbpAuthorize(AppPermissions.Pages_JobTags_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _jobTagRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetJobTagsToExcel(GetAllJobTagsForExcelInput input)
        {

            var filteredJobTags = _jobTagRepository.GetAll()
                        .Include(e => e.JobFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTag.Contains(input.Filter) || e.TagValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagFilter), e => e.CustomTag.Contains(input.CustomTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagValueFilter), e => e.TagValue.Contains(input.TagValueFilter))
                        .WhereIf(input.VerifiedFilter.HasValue && input.VerifiedFilter > -1, e => (input.VerifiedFilter == 1 && e.Verified) || (input.VerifiedFilter == 0 && !e.Verified))
                        .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                        .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobFk != null && e.JobFk.Title == input.JobTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var query = (from o in filteredJobTags
                         join o1 in _lookup_jobRepository.GetAll() on o.JobId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetJobTagForViewDto()
                         {
                             JobTag = new JobTagDto
                             {
                                 CustomTag = o.CustomTag,
                                 TagValue = o.TagValue,
                                 Verified = o.Verified,
                                 Sequence = o.Sequence,
                                 Id = o.Id
                             },
                             JobTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                             MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var jobTagListDtos = await query.ToListAsync();

            return _jobTagsExcelExporter.ExportToFile(jobTagListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_JobTags)]
        public async Task<PagedResultDto<JobTagJobLookupTableDto>> GetAllJobForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_jobRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Title != null && e.Title.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var jobList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobTagJobLookupTableDto>();
            foreach (var job in jobList)
            {
                lookupTableDtoList.Add(new JobTagJobLookupTableDto
                {
                    Id = job.Id,
                    DisplayName = job.Title?.ToString()
                });
            }

            return new PagedResultDto<JobTagJobLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_JobTags)]
        public async Task<PagedResultDto<JobTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobTagMasterTagCategoryLookupTableDto>();
            foreach (var masterTagCategory in masterTagCategoryList)
            {
                lookupTableDtoList.Add(new JobTagMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<JobTagMasterTagCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_JobTags)]
        public async Task<PagedResultDto<JobTagMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobTagMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new JobTagMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<JobTagMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}