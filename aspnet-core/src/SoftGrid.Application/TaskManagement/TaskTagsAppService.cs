using SoftGrid.TaskManagement;
using SoftGrid.LookupData;
using SoftGrid.LookupData;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.TaskManagement.Exporting;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.TaskManagement
{
    [AbpAuthorize(AppPermissions.Pages_TaskTags)]
    public class TaskTagsAppService : SoftGridAppServiceBase, ITaskTagsAppService
    {
        private readonly IRepository<TaskTag, long> _taskTagRepository;
        private readonly ITaskTagsExcelExporter _taskTagsExcelExporter;
        private readonly IRepository<TaskEvent, long> _lookup_taskEventRepository;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;
        private readonly IRepository<MasterTag, long> _lookup_masterTagRepository;

        public TaskTagsAppService(IRepository<TaskTag, long> taskTagRepository, ITaskTagsExcelExporter taskTagsExcelExporter, IRepository<TaskEvent, long> lookup_taskEventRepository, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository, IRepository<MasterTag, long> lookup_masterTagRepository)
        {
            _taskTagRepository = taskTagRepository;
            _taskTagsExcelExporter = taskTagsExcelExporter;
            _lookup_taskEventRepository = lookup_taskEventRepository;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;
            _lookup_masterTagRepository = lookup_masterTagRepository;

        }

        public async Task<PagedResultDto<GetTaskTagForViewDto>> GetAll(GetAllTaskTagsInput input)
        {

            var filteredTaskTags = _taskTagRepository.GetAll()
                        .Include(e => e.TaskEventFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTag.Contains(input.Filter) || e.TagValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagFilter), e => e.CustomTag.Contains(input.CustomTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagValueFilter), e => e.TagValue.Contains(input.TagValueFilter))
                        .WhereIf(input.VerfiedFilter.HasValue && input.VerfiedFilter > -1, e => (input.VerfiedFilter == 1 && e.Verfied) || (input.VerfiedFilter == 0 && !e.Verfied))
                        .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                        .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var pagedAndFilteredTaskTags = filteredTaskTags
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var taskTags = from o in pagedAndFilteredTaskTags
                           join o1 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           join o3 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o3.Id into j3
                           from s3 in j3.DefaultIfEmpty()

                           select new
                           {

                               o.CustomTag,
                               o.TagValue,
                               o.Verfied,
                               o.Sequence,
                               Id = o.Id,
                               TaskEventName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                               MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                               MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                           };

            var totalCount = await filteredTaskTags.CountAsync();

            var dbList = await taskTags.ToListAsync();
            var results = new List<GetTaskTagForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTaskTagForViewDto()
                {
                    TaskTag = new TaskTagDto
                    {

                        CustomTag = o.CustomTag,
                        TagValue = o.TagValue,
                        Verfied = o.Verfied,
                        Sequence = o.Sequence,
                        Id = o.Id,
                    },
                    TaskEventName = o.TaskEventName,
                    MasterTagCategoryName = o.MasterTagCategoryName,
                    MasterTagName = o.MasterTagName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTaskTagForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetTaskTagForViewDto> GetTaskTagForView(long id)
        {
            var taskTag = await _taskTagRepository.GetAsync(id);

            var output = new GetTaskTagForViewDto { TaskTag = ObjectMapper.Map<TaskTagDto>(taskTag) };

            if (output.TaskTag.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.TaskTag.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            if (output.TaskTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.TaskTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.TaskTag.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.TaskTag.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTags_Edit)]
        public async Task<GetTaskTagForEditOutput> GetTaskTagForEdit(EntityDto<long> input)
        {
            var taskTag = await _taskTagRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTaskTagForEditOutput { TaskTag = ObjectMapper.Map<CreateOrEditTaskTagDto>(taskTag) };

            if (output.TaskTag.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.TaskTag.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            if (output.TaskTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.TaskTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.TaskTag.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.TaskTag.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTaskTagDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TaskTags_Create)]
        protected virtual async Task Create(CreateOrEditTaskTagDto input)
        {
            var taskTag = ObjectMapper.Map<TaskTag>(input);

            if (AbpSession.TenantId != null)
            {
                taskTag.TenantId = (int?)AbpSession.TenantId;
            }

            await _taskTagRepository.InsertAsync(taskTag);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskTags_Edit)]
        protected virtual async Task Update(CreateOrEditTaskTagDto input)
        {
            var taskTag = await _taskTagRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, taskTag);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskTags_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _taskTagRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTaskTagsToExcel(GetAllTaskTagsForExcelInput input)
        {

            var filteredTaskTags = _taskTagRepository.GetAll()
                        .Include(e => e.TaskEventFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTag.Contains(input.Filter) || e.TagValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagFilter), e => e.CustomTag.Contains(input.CustomTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagValueFilter), e => e.TagValue.Contains(input.TagValueFilter))
                        .WhereIf(input.VerfiedFilter.HasValue && input.VerfiedFilter > -1, e => (input.VerfiedFilter == 1 && e.Verfied) || (input.VerfiedFilter == 0 && !e.Verfied))
                        .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                        .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var query = (from o in filteredTaskTags
                         join o1 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetTaskTagForViewDto()
                         {
                             TaskTag = new TaskTagDto
                             {
                                 CustomTag = o.CustomTag,
                                 TagValue = o.TagValue,
                                 Verfied = o.Verfied,
                                 Sequence = o.Sequence,
                                 Id = o.Id
                             },
                             TaskEventName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var taskTagListDtos = await query.ToListAsync();

            return _taskTagsExcelExporter.ExportToFile(taskTagListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTags)]
        public async Task<PagedResultDto<TaskTagTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_taskEventRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var taskEventList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TaskTagTaskEventLookupTableDto>();
            foreach (var taskEvent in taskEventList)
            {
                lookupTableDtoList.Add(new TaskTagTaskEventLookupTableDto
                {
                    Id = taskEvent.Id,
                    DisplayName = taskEvent.Name?.ToString()
                });
            }

            return new PagedResultDto<TaskTagTaskEventLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTags)]
        public async Task<PagedResultDto<TaskTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TaskTagMasterTagCategoryLookupTableDto>();
            foreach (var masterTagCategory in masterTagCategoryList)
            {
                lookupTableDtoList.Add(new TaskTagMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<TaskTagMasterTagCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTags)]
        public async Task<PagedResultDto<TaskTagMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TaskTagMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new TaskTagMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<TaskTagMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}