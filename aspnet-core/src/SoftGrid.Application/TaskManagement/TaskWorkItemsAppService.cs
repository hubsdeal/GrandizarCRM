using SoftGrid.TaskManagement;
using SoftGrid.CRM;

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
    [AbpAuthorize(AppPermissions.Pages_TaskWorkItems)]
    public class TaskWorkItemsAppService : SoftGridAppServiceBase, ITaskWorkItemsAppService
    {
        private readonly IRepository<TaskWorkItem, long> _taskWorkItemRepository;
        private readonly ITaskWorkItemsExcelExporter _taskWorkItemsExcelExporter;
        private readonly IRepository<TaskEvent, long> _lookup_taskEventRepository;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;

        public TaskWorkItemsAppService(IRepository<TaskWorkItem, long> taskWorkItemRepository, ITaskWorkItemsExcelExporter taskWorkItemsExcelExporter, IRepository<TaskEvent, long> lookup_taskEventRepository, IRepository<Employee, long> lookup_employeeRepository)
        {
            _taskWorkItemRepository = taskWorkItemRepository;
            _taskWorkItemsExcelExporter = taskWorkItemsExcelExporter;
            _lookup_taskEventRepository = lookup_taskEventRepository;
            _lookup_employeeRepository = lookup_employeeRepository;

        }

        public async Task<PagedResultDto<GetTaskWorkItemForViewDto>> GetAll(GetAllTaskWorkItemsInput input)
        {

            var filteredTaskWorkItems = _taskWorkItemRepository.GetAll()
                        .Include(e => e.TaskEventFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.EstimatedHours.Contains(input.Filter) || e.ActualHours.Contains(input.Filter) || e.StartTime.Contains(input.Filter) || e.EndTime.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EstimatedHoursFilter), e => e.EstimatedHours.Contains(input.EstimatedHoursFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActualHoursFilter), e => e.ActualHours.Contains(input.ActualHoursFilter))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StartTimeFilter), e => e.StartTime.Contains(input.StartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EndTimeFilter), e => e.EndTime.Contains(input.EndTimeFilter))
                        .WhereIf(input.OpenOrClosedFilter.HasValue && input.OpenOrClosedFilter > -1, e => (input.OpenOrClosedFilter == 1 && e.OpenOrClosed) || (input.OpenOrClosedFilter == 0 && !e.OpenOrClosed))
                        .WhereIf(input.MinCompletionPercentageFilter != null, e => e.CompletionPercentage >= input.MinCompletionPercentageFilter)
                        .WhereIf(input.MaxCompletionPercentageFilter != null, e => e.CompletionPercentage <= input.MaxCompletionPercentageFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var pagedAndFilteredTaskWorkItems = filteredTaskWorkItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var taskWorkItems = from o in pagedAndFilteredTaskWorkItems
                                join o1 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                select new
                                {

                                    o.Name,
                                    o.EstimatedHours,
                                    o.ActualHours,
                                    o.StartDate,
                                    o.EndDate,
                                    o.StartTime,
                                    o.EndTime,
                                    o.OpenOrClosed,
                                    o.CompletionPercentage,
                                    Id = o.Id,
                                    TaskEventName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                    EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                };

            var totalCount = await filteredTaskWorkItems.CountAsync();

            var dbList = await taskWorkItems.ToListAsync();
            var results = new List<GetTaskWorkItemForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTaskWorkItemForViewDto()
                {
                    TaskWorkItem = new TaskWorkItemDto
                    {

                        Name = o.Name,
                        EstimatedHours = o.EstimatedHours,
                        ActualHours = o.ActualHours,
                        StartDate = o.StartDate,
                        EndDate = o.EndDate,
                        StartTime = o.StartTime,
                        EndTime = o.EndTime,
                        OpenOrClosed = o.OpenOrClosed,
                        CompletionPercentage = o.CompletionPercentage,
                        Id = o.Id,
                    },
                    TaskEventName = o.TaskEventName,
                    EmployeeName = o.EmployeeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTaskWorkItemForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetTaskWorkItemForViewDto> GetTaskWorkItemForView(long id)
        {
            var taskWorkItem = await _taskWorkItemRepository.GetAsync(id);

            var output = new GetTaskWorkItemForViewDto { TaskWorkItem = ObjectMapper.Map<TaskWorkItemDto>(taskWorkItem) };

            if (output.TaskWorkItem.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.TaskWorkItem.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            if (output.TaskWorkItem.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.TaskWorkItem.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TaskWorkItems_Edit)]
        public async Task<GetTaskWorkItemForEditOutput> GetTaskWorkItemForEdit(EntityDto<long> input)
        {
            var taskWorkItem = await _taskWorkItemRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTaskWorkItemForEditOutput { TaskWorkItem = ObjectMapper.Map<CreateOrEditTaskWorkItemDto>(taskWorkItem) };

            if (output.TaskWorkItem.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.TaskWorkItem.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            if (output.TaskWorkItem.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.TaskWorkItem.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTaskWorkItemDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TaskWorkItems_Create)]
        protected virtual async Task Create(CreateOrEditTaskWorkItemDto input)
        {
            var taskWorkItem = ObjectMapper.Map<TaskWorkItem>(input);

            if (AbpSession.TenantId != null)
            {
                taskWorkItem.TenantId = (int?)AbpSession.TenantId;
            }

            await _taskWorkItemRepository.InsertAsync(taskWorkItem);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskWorkItems_Edit)]
        protected virtual async Task Update(CreateOrEditTaskWorkItemDto input)
        {
            var taskWorkItem = await _taskWorkItemRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, taskWorkItem);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskWorkItems_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _taskWorkItemRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTaskWorkItemsToExcel(GetAllTaskWorkItemsForExcelInput input)
        {

            var filteredTaskWorkItems = _taskWorkItemRepository.GetAll()
                        .Include(e => e.TaskEventFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.EstimatedHours.Contains(input.Filter) || e.ActualHours.Contains(input.Filter) || e.StartTime.Contains(input.Filter) || e.EndTime.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EstimatedHoursFilter), e => e.EstimatedHours.Contains(input.EstimatedHoursFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ActualHoursFilter), e => e.ActualHours.Contains(input.ActualHoursFilter))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StartTimeFilter), e => e.StartTime.Contains(input.StartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EndTimeFilter), e => e.EndTime.Contains(input.EndTimeFilter))
                        .WhereIf(input.OpenOrClosedFilter.HasValue && input.OpenOrClosedFilter > -1, e => (input.OpenOrClosedFilter == 1 && e.OpenOrClosed) || (input.OpenOrClosedFilter == 0 && !e.OpenOrClosed))
                        .WhereIf(input.MinCompletionPercentageFilter != null, e => e.CompletionPercentage >= input.MinCompletionPercentageFilter)
                        .WhereIf(input.MaxCompletionPercentageFilter != null, e => e.CompletionPercentage <= input.MaxCompletionPercentageFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var query = (from o in filteredTaskWorkItems
                         join o1 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetTaskWorkItemForViewDto()
                         {
                             TaskWorkItem = new TaskWorkItemDto
                             {
                                 Name = o.Name,
                                 EstimatedHours = o.EstimatedHours,
                                 ActualHours = o.ActualHours,
                                 StartDate = o.StartDate,
                                 EndDate = o.EndDate,
                                 StartTime = o.StartTime,
                                 EndTime = o.EndTime,
                                 OpenOrClosed = o.OpenOrClosed,
                                 CompletionPercentage = o.CompletionPercentage,
                                 Id = o.Id
                             },
                             TaskEventName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var taskWorkItemListDtos = await query.ToListAsync();

            return _taskWorkItemsExcelExporter.ExportToFile(taskWorkItemListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_TaskWorkItems)]
        public async Task<PagedResultDto<TaskWorkItemTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_taskEventRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var taskEventList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TaskWorkItemTaskEventLookupTableDto>();
            foreach (var taskEvent in taskEventList)
            {
                lookupTableDtoList.Add(new TaskWorkItemTaskEventLookupTableDto
                {
                    Id = taskEvent.Id,
                    DisplayName = taskEvent.Name?.ToString()
                });
            }

            return new PagedResultDto<TaskWorkItemTaskEventLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_TaskWorkItems)]
        public async Task<PagedResultDto<TaskWorkItemEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TaskWorkItemEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new TaskWorkItemEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<TaskWorkItemEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}