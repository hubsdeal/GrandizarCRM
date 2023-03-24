using SoftGrid.TaskManagement;

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
    [AbpAuthorize(AppPermissions.Pages_TaskEvents)]
    public class TaskEventsAppService : SoftGridAppServiceBase, ITaskEventsAppService
    {
        private readonly IRepository<TaskEvent, long> _taskEventRepository;
        private readonly ITaskEventsExcelExporter _taskEventsExcelExporter;
        private readonly IRepository<TaskStatus, long> _lookup_taskStatusRepository;

        public TaskEventsAppService(IRepository<TaskEvent, long> taskEventRepository, ITaskEventsExcelExporter taskEventsExcelExporter, IRepository<TaskStatus, long> lookup_taskStatusRepository)
        {
            _taskEventRepository = taskEventRepository;
            _taskEventsExcelExporter = taskEventsExcelExporter;
            _lookup_taskStatusRepository = lookup_taskStatusRepository;

        }

        public async Task<PagedResultDto<GetTaskEventForViewDto>> GetAll(GetAllTaskEventsInput input)
        {

            var filteredTaskEvents = _taskEventRepository.GetAll()
                        .Include(e => e.TaskStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.StartTime.Contains(input.Filter) || e.EndTime.Contains(input.Filter) || e.ActualTime.Contains(input.Filter) || e.EstimatedTime.Contains(input.Filter) || e.HourAndMinutes.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => (input.StatusFilter == 1 && e.Status) || (input.StatusFilter == 0 && !e.Status))
                        .WhereIf(input.PriorityFilter.HasValue && input.PriorityFilter > -1, e => (input.PriorityFilter == 1 && e.Priority) || (input.PriorityFilter == 0 && !e.Priority))
                        .WhereIf(input.MinEventDateFilter != null, e => e.EventDate >= input.MinEventDateFilter)
                        .WhereIf(input.MaxEventDateFilter != null, e => e.EventDate <= input.MaxEventDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StartTimeFilter), e => e.StartTime.Contains(input.StartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EndTimeFilter), e => e.EndTime.Contains(input.EndTimeFilter))
                        .WhereIf(input.TemplateFilter.HasValue && input.TemplateFilter > -1, e => (input.TemplateFilter == 1 && e.Template) || (input.TemplateFilter == 0 && !e.Template))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActualTimeFilter), e => e.ActualTime.Contains(input.ActualTimeFilter))
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EstimatedTimeFilter), e => e.EstimatedTime.Contains(input.EstimatedTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HourAndMinutesFilter), e => e.HourAndMinutes.Contains(input.HourAndMinutesFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskStatusNameFilter), e => e.TaskStatusFk != null && e.TaskStatusFk.Name == input.TaskStatusNameFilter);

            var pagedAndFilteredTaskEvents = filteredTaskEvents
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var taskEvents = from o in pagedAndFilteredTaskEvents
                             join o1 in _lookup_taskStatusRepository.GetAll() on o.TaskStatusId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             select new
                             {

                                 o.Name,
                                 o.Description,
                                 o.Status,
                                 o.Priority,
                                 o.EventDate,
                                 o.StartTime,
                                 o.EndTime,
                                 o.Template,
                                 o.ActualTime,
                                 o.EndDate,
                                 o.EstimatedTime,
                                 o.HourAndMinutes,
                                 Id = o.Id,
                                 TaskStatusName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                             };

            var totalCount = await filteredTaskEvents.CountAsync();

            var dbList = await taskEvents.ToListAsync();
            var results = new List<GetTaskEventForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTaskEventForViewDto()
                {
                    TaskEvent = new TaskEventDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Status = o.Status,
                        Priority = o.Priority,
                        EventDate = o.EventDate,
                        StartTime = o.StartTime,
                        EndTime = o.EndTime,
                        Template = o.Template,
                        ActualTime = o.ActualTime,
                        EndDate = o.EndDate,
                        EstimatedTime = o.EstimatedTime,
                        HourAndMinutes = o.HourAndMinutes,
                        Id = o.Id,
                    },
                    TaskStatusName = o.TaskStatusName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTaskEventForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetTaskEventForViewDto> GetTaskEventForView(long id)
        {
            var taskEvent = await _taskEventRepository.GetAsync(id);

            var output = new GetTaskEventForViewDto { TaskEvent = ObjectMapper.Map<TaskEventDto>(taskEvent) };

            if (output.TaskEvent.TaskStatusId != null)
            {
                var _lookupTaskStatus = await _lookup_taskStatusRepository.FirstOrDefaultAsync((long)output.TaskEvent.TaskStatusId);
                output.TaskStatusName = _lookupTaskStatus?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TaskEvents_Edit)]
        public async Task<GetTaskEventForEditOutput> GetTaskEventForEdit(EntityDto<long> input)
        {
            var taskEvent = await _taskEventRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTaskEventForEditOutput { TaskEvent = ObjectMapper.Map<CreateOrEditTaskEventDto>(taskEvent) };

            if (output.TaskEvent.TaskStatusId != null)
            {
                var _lookupTaskStatus = await _lookup_taskStatusRepository.FirstOrDefaultAsync((long)output.TaskEvent.TaskStatusId);
                output.TaskStatusName = _lookupTaskStatus?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTaskEventDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TaskEvents_Create)]
        protected virtual async Task Create(CreateOrEditTaskEventDto input)
        {
            var taskEvent = ObjectMapper.Map<TaskEvent>(input);

            if (AbpSession.TenantId != null)
            {
                taskEvent.TenantId = (int?)AbpSession.TenantId;
            }

            await _taskEventRepository.InsertAsync(taskEvent);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskEvents_Edit)]
        protected virtual async Task Update(CreateOrEditTaskEventDto input)
        {
            var taskEvent = await _taskEventRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, taskEvent);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskEvents_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _taskEventRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTaskEventsToExcel(GetAllTaskEventsForExcelInput input)
        {

            var filteredTaskEvents = _taskEventRepository.GetAll()
                        .Include(e => e.TaskStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.StartTime.Contains(input.Filter) || e.EndTime.Contains(input.Filter) || e.ActualTime.Contains(input.Filter) || e.EstimatedTime.Contains(input.Filter) || e.HourAndMinutes.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => (input.StatusFilter == 1 && e.Status) || (input.StatusFilter == 0 && !e.Status))
                        .WhereIf(input.PriorityFilter.HasValue && input.PriorityFilter > -1, e => (input.PriorityFilter == 1 && e.Priority) || (input.PriorityFilter == 0 && !e.Priority))
                        .WhereIf(input.MinEventDateFilter != null, e => e.EventDate >= input.MinEventDateFilter)
                        .WhereIf(input.MaxEventDateFilter != null, e => e.EventDate <= input.MaxEventDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StartTimeFilter), e => e.StartTime.Contains(input.StartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EndTimeFilter), e => e.EndTime.Contains(input.EndTimeFilter))
                        .WhereIf(input.TemplateFilter.HasValue && input.TemplateFilter > -1, e => (input.TemplateFilter == 1 && e.Template) || (input.TemplateFilter == 0 && !e.Template))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActualTimeFilter), e => e.ActualTime.Contains(input.ActualTimeFilter))
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EstimatedTimeFilter), e => e.EstimatedTime.Contains(input.EstimatedTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HourAndMinutesFilter), e => e.HourAndMinutes.Contains(input.HourAndMinutesFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskStatusNameFilter), e => e.TaskStatusFk != null && e.TaskStatusFk.Name == input.TaskStatusNameFilter);

            var query = (from o in filteredTaskEvents
                         join o1 in _lookup_taskStatusRepository.GetAll() on o.TaskStatusId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetTaskEventForViewDto()
                         {
                             TaskEvent = new TaskEventDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Status = o.Status,
                                 Priority = o.Priority,
                                 EventDate = o.EventDate,
                                 StartTime = o.StartTime,
                                 EndTime = o.EndTime,
                                 Template = o.Template,
                                 ActualTime = o.ActualTime,
                                 EndDate = o.EndDate,
                                 EstimatedTime = o.EstimatedTime,
                                 HourAndMinutes = o.HourAndMinutes,
                                 Id = o.Id
                             },
                             TaskStatusName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var taskEventListDtos = await query.ToListAsync();

            return _taskEventsExcelExporter.ExportToFile(taskEventListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_TaskEvents)]
        public async Task<List<TaskEventTaskStatusLookupTableDto>> GetAllTaskStatusForTableDropdown()
        {
            return await _lookup_taskStatusRepository.GetAll()
                .Select(taskStatus => new TaskEventTaskStatusLookupTableDto
                {
                    Id = taskStatus.Id,
                    DisplayName = taskStatus == null || taskStatus.Name == null ? "" : taskStatus.Name.ToString()
                }).ToListAsync();
        }

    }
}