using SoftGrid.LookupData;
using SoftGrid.LookupData;

using SoftGrid.Shop.Enums;

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
    [AbpAuthorize(AppPermissions.Pages_JobMasterTagSettings)]
    public class JobMasterTagSettingsAppService : SoftGridAppServiceBase, IJobMasterTagSettingsAppService
    {
        private readonly IRepository<JobMasterTagSetting, long> _jobMasterTagSettingRepository;
        private readonly IJobMasterTagSettingsExcelExporter _jobMasterTagSettingsExcelExporter;
        private readonly IRepository<MasterTag, long> _lookup_masterTagRepository;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;

        public JobMasterTagSettingsAppService(IRepository<JobMasterTagSetting, long> jobMasterTagSettingRepository, IJobMasterTagSettingsExcelExporter jobMasterTagSettingsExcelExporter, IRepository<MasterTag, long> lookup_masterTagRepository, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository)
        {
            _jobMasterTagSettingRepository = jobMasterTagSettingRepository;
            _jobMasterTagSettingsExcelExporter = jobMasterTagSettingsExcelExporter;
            _lookup_masterTagRepository = lookup_masterTagRepository;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;

        }

        public async Task<PagedResultDto<GetJobMasterTagSettingForViewDto>> GetAll(GetAllJobMasterTagSettingsInput input)
        {
            var answerTypeIdFilter = input.AnswerTypeIdFilter.HasValue
                        ? (AnswerType)input.AnswerTypeIdFilter
                        : default;

            var filteredJobMasterTagSettings = _jobMasterTagSettingRepository.GetAll()
                        .Include(e => e.JobCategoryFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTagTitle.Contains(input.Filter) || e.CustomTagChatQuestion.Contains(input.Filter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.DisplayPublicFilter.HasValue && input.DisplayPublicFilter > -1, e => (input.DisplayPublicFilter == 1 && e.DisplayPublic) || (input.DisplayPublicFilter == 0 && !e.DisplayPublic))
                        .WhereIf(input.AnswerTypeIdFilter.HasValue && input.AnswerTypeIdFilter > -1, e => e.AnswerTypeId == answerTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagTitleFilter), e => e.CustomTagTitle.Contains(input.CustomTagTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagChatQuestionFilter), e => e.CustomTagChatQuestion.Contains(input.CustomTagChatQuestionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.JobCategoryFk != null && e.JobCategoryFk.Name == input.MasterTagNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter);

            var pagedAndFilteredJobMasterTagSettings = filteredJobMasterTagSettings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var jobMasterTagSettings = from o in pagedAndFilteredJobMasterTagSettings
                                       join o1 in _lookup_masterTagRepository.GetAll() on o.JobCategoryId equals o1.Id into j1
                                       from s1 in j1.DefaultIfEmpty()

                                       join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                                       from s2 in j2.DefaultIfEmpty()

                                       select new
                                       {

                                           o.DisplaySequence,
                                           o.DisplayPublic,
                                           o.AnswerTypeId,
                                           o.CustomTagTitle,
                                           o.CustomTagChatQuestion,
                                           Id = o.Id,
                                           MasterTagName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                           MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                       };

            var totalCount = await filteredJobMasterTagSettings.CountAsync();

            var dbList = await jobMasterTagSettings.ToListAsync();
            var results = new List<GetJobMasterTagSettingForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetJobMasterTagSettingForViewDto()
                {
                    JobMasterTagSetting = new JobMasterTagSettingDto
                    {

                        DisplaySequence = o.DisplaySequence,
                        DisplayPublic = o.DisplayPublic,
                        AnswerTypeId = o.AnswerTypeId,
                        CustomTagTitle = o.CustomTagTitle,
                        CustomTagChatQuestion = o.CustomTagChatQuestion,
                        Id = o.Id,
                    },
                    MasterTagName = o.MasterTagName,
                    MasterTagCategoryName = o.MasterTagCategoryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetJobMasterTagSettingForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetJobMasterTagSettingForViewDto> GetJobMasterTagSettingForView(long id)
        {
            var jobMasterTagSetting = await _jobMasterTagSettingRepository.GetAsync(id);

            var output = new GetJobMasterTagSettingForViewDto { JobMasterTagSetting = ObjectMapper.Map<JobMasterTagSettingDto>(jobMasterTagSetting) };

            if (output.JobMasterTagSetting.JobCategoryId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.JobMasterTagSetting.JobCategoryId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            if (output.JobMasterTagSetting.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.JobMasterTagSetting.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_JobMasterTagSettings_Edit)]
        public async Task<GetJobMasterTagSettingForEditOutput> GetJobMasterTagSettingForEdit(EntityDto<long> input)
        {
            var jobMasterTagSetting = await _jobMasterTagSettingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetJobMasterTagSettingForEditOutput { JobMasterTagSetting = ObjectMapper.Map<CreateOrEditJobMasterTagSettingDto>(jobMasterTagSetting) };

            if (output.JobMasterTagSetting.JobCategoryId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.JobMasterTagSetting.JobCategoryId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            if (output.JobMasterTagSetting.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.JobMasterTagSetting.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditJobMasterTagSettingDto input)
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

        [AbpAuthorize(AppPermissions.Pages_JobMasterTagSettings_Create)]
        protected virtual async Task Create(CreateOrEditJobMasterTagSettingDto input)
        {
            var jobMasterTagSetting = ObjectMapper.Map<JobMasterTagSetting>(input);

            if (AbpSession.TenantId != null)
            {
                jobMasterTagSetting.TenantId = (int?)AbpSession.TenantId;
            }

            await _jobMasterTagSettingRepository.InsertAsync(jobMasterTagSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_JobMasterTagSettings_Edit)]
        protected virtual async Task Update(CreateOrEditJobMasterTagSettingDto input)
        {
            var jobMasterTagSetting = await _jobMasterTagSettingRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, jobMasterTagSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_JobMasterTagSettings_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _jobMasterTagSettingRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetJobMasterTagSettingsToExcel(GetAllJobMasterTagSettingsForExcelInput input)
        {
            var answerTypeIdFilter = input.AnswerTypeIdFilter.HasValue
                        ? (AnswerType)input.AnswerTypeIdFilter
                        : default;

            var filteredJobMasterTagSettings = _jobMasterTagSettingRepository.GetAll()
                        .Include(e => e.JobCategoryFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTagTitle.Contains(input.Filter) || e.CustomTagChatQuestion.Contains(input.Filter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.DisplayPublicFilter.HasValue && input.DisplayPublicFilter > -1, e => (input.DisplayPublicFilter == 1 && e.DisplayPublic) || (input.DisplayPublicFilter == 0 && !e.DisplayPublic))
                        .WhereIf(input.AnswerTypeIdFilter.HasValue && input.AnswerTypeIdFilter > -1, e => e.AnswerTypeId == answerTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagTitleFilter), e => e.CustomTagTitle.Contains(input.CustomTagTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagChatQuestionFilter), e => e.CustomTagChatQuestion.Contains(input.CustomTagChatQuestionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.JobCategoryFk != null && e.JobCategoryFk.Name == input.MasterTagNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter);

            var query = (from o in filteredJobMasterTagSettings
                         join o1 in _lookup_masterTagRepository.GetAll() on o.JobCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetJobMasterTagSettingForViewDto()
                         {
                             JobMasterTagSetting = new JobMasterTagSettingDto
                             {
                                 DisplaySequence = o.DisplaySequence,
                                 DisplayPublic = o.DisplayPublic,
                                 AnswerTypeId = o.AnswerTypeId,
                                 CustomTagTitle = o.CustomTagTitle,
                                 CustomTagChatQuestion = o.CustomTagChatQuestion,
                                 Id = o.Id
                             },
                             MasterTagName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var jobMasterTagSettingListDtos = await query.ToListAsync();

            return _jobMasterTagSettingsExcelExporter.ExportToFile(jobMasterTagSettingListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_JobMasterTagSettings)]
        public async Task<PagedResultDto<JobMasterTagSettingMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobMasterTagSettingMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new JobMasterTagSettingMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<JobMasterTagSettingMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_JobMasterTagSettings)]
        public async Task<PagedResultDto<JobMasterTagSettingMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobMasterTagSettingMasterTagCategoryLookupTableDto>();
            foreach (var masterTagCategory in masterTagCategoryList)
            {
                lookupTableDtoList.Add(new JobMasterTagSettingMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<JobMasterTagSettingMasterTagCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}