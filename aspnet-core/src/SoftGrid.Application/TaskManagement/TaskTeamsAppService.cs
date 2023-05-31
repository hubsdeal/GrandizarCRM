using SoftGrid.TaskManagement;
using SoftGrid.CRM;
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
    [AbpAuthorize(AppPermissions.Pages_TaskTeams)]
    public class TaskTeamsAppService : SoftGridAppServiceBase, ITaskTeamsAppService
    {
        private readonly IRepository<TaskTeam, long> _taskTeamRepository;
        private readonly ITaskTeamsExcelExporter _taskTeamsExcelExporter;
        private readonly IRepository<TaskEvent, long> _lookup_taskEventRepository;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;

        public TaskTeamsAppService(IRepository<TaskTeam, long> taskTeamRepository, ITaskTeamsExcelExporter taskTeamsExcelExporter, IRepository<TaskEvent, long> lookup_taskEventRepository, IRepository<Employee, long> lookup_employeeRepository, IRepository<Contact, long> lookup_contactRepository)
        {
            _taskTeamRepository = taskTeamRepository;
            _taskTeamsExcelExporter = taskTeamsExcelExporter;
            _lookup_taskEventRepository = lookup_taskEventRepository;
            _lookup_employeeRepository = lookup_employeeRepository;
            _lookup_contactRepository = lookup_contactRepository;

        }

        public async Task<PagedResultDto<GetTaskTeamForViewDto>> GetAll(GetAllTaskTeamsInput input)
        {

            var filteredTaskTeams = _taskTeamRepository.GetAll()
                        .Include(e => e.TaskEventFk)
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.StartTime.Contains(input.Filter) || e.EndTime.Contains(input.Filter) || e.HourMinutes.Contains(input.Filter) || e.EstimatedHour.Contains(input.Filter) || e.SubTaskTitle.Contains(input.Filter))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StartTimeFilter), e => e.StartTime.Contains(input.StartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EndTimeFilter), e => e.EndTime.Contains(input.EndTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HourMinutesFilter), e => e.HourMinutes.Contains(input.HourMinutesFilter))
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.IsPrimaryFilter.HasValue && input.IsPrimaryFilter > -1, e => (input.IsPrimaryFilter == 1 && e.IsPrimary) || (input.IsPrimaryFilter == 0 && !e.IsPrimary))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EstimatedHourFilter), e => e.EstimatedHour.Contains(input.EstimatedHourFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubTaskTitleFilter), e => e.SubTaskTitle.Contains(input.SubTaskTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var pagedAndFilteredTaskTeams = filteredTaskTeams
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var taskTeams = from o in pagedAndFilteredTaskTeams
                            join o1 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            join o3 in _lookup_contactRepository.GetAll() on o.ContactId equals o3.Id into j3
                            from s3 in j3.DefaultIfEmpty()

                            select new
                            {

                                o.StartDate,
                                o.StartTime,
                                o.EndTime,
                                o.HourMinutes,
                                o.EndDate,
                                o.IsPrimary,
                                o.EstimatedHour,
                                o.SubTaskTitle,
                                Id = o.Id,
                                TaskEventName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                ContactFullName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString()
                            };

            var totalCount = await filteredTaskTeams.CountAsync();

            var dbList = await taskTeams.ToListAsync();
            var results = new List<GetTaskTeamForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTaskTeamForViewDto()
                {
                    TaskTeam = new TaskTeamDto
                    {

                        StartDate = o.StartDate,
                        StartTime = o.StartTime,
                        EndTime = o.EndTime,
                        HourMinutes = o.HourMinutes,
                        EndDate = o.EndDate,
                        IsPrimary = o.IsPrimary,
                        EstimatedHour = o.EstimatedHour,
                        SubTaskTitle = o.SubTaskTitle,
                        Id = o.Id,
                    },
                    TaskEventName = o.TaskEventName,
                    EmployeeName = o.EmployeeName,
                    ContactFullName = o.ContactFullName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTaskTeamForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetTaskTeamForViewDto> GetTaskTeamForView(long id)
        {
            var taskTeam = await _taskTeamRepository.GetAsync(id);

            var output = new GetTaskTeamForViewDto { TaskTeam = ObjectMapper.Map<TaskTeamDto>(taskTeam) };

            if (output.TaskTeam.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.TaskTeam.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            if (output.TaskTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.TaskTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.TaskTeam.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.TaskTeam.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTeams_Edit)]
        public async Task<GetTaskTeamForEditOutput> GetTaskTeamForEdit(EntityDto<long> input)
        {
            var taskTeam = await _taskTeamRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTaskTeamForEditOutput { TaskTeam = ObjectMapper.Map<CreateOrEditTaskTeamDto>(taskTeam) };

            if (output.TaskTeam.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.TaskTeam.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            if (output.TaskTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.TaskTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.TaskTeam.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.TaskTeam.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTaskTeamDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TaskTeams_Create)]
        protected virtual async Task Create(CreateOrEditTaskTeamDto input)
        {
            var taskTeam = ObjectMapper.Map<TaskTeam>(input);

            if (AbpSession.TenantId != null)
            {
                taskTeam.TenantId = (int?)AbpSession.TenantId;
            }

            await _taskTeamRepository.InsertAsync(taskTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskTeams_Edit)]
        protected virtual async Task Update(CreateOrEditTaskTeamDto input)
        {
            var taskTeam = await _taskTeamRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, taskTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskTeams_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _taskTeamRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTaskTeamsToExcel(GetAllTaskTeamsForExcelInput input)
        {

            var filteredTaskTeams = _taskTeamRepository.GetAll()
                        .Include(e => e.TaskEventFk)
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.StartTime.Contains(input.Filter) || e.EndTime.Contains(input.Filter) || e.HourMinutes.Contains(input.Filter) || e.EstimatedHour.Contains(input.Filter) || e.SubTaskTitle.Contains(input.Filter))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StartTimeFilter), e => e.StartTime.Contains(input.StartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EndTimeFilter), e => e.EndTime.Contains(input.EndTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HourMinutesFilter), e => e.HourMinutes.Contains(input.HourMinutesFilter))
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.IsPrimaryFilter.HasValue && input.IsPrimaryFilter > -1, e => (input.IsPrimaryFilter == 1 && e.IsPrimary) || (input.IsPrimaryFilter == 0 && !e.IsPrimary))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EstimatedHourFilter), e => e.EstimatedHour.Contains(input.EstimatedHourFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubTaskTitleFilter), e => e.SubTaskTitle.Contains(input.SubTaskTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var query = (from o in filteredTaskTeams
                         join o1 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_contactRepository.GetAll() on o.ContactId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetTaskTeamForViewDto()
                         {
                             TaskTeam = new TaskTeamDto
                             {
                                 StartDate = o.StartDate,
                                 StartTime = o.StartTime,
                                 EndTime = o.EndTime,
                                 HourMinutes = o.HourMinutes,
                                 EndDate = o.EndDate,
                                 IsPrimary = o.IsPrimary,
                                 EstimatedHour = o.EstimatedHour,
                                 SubTaskTitle = o.SubTaskTitle,
                                 Id = o.Id
                             },
                             TaskEventName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ContactFullName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString()
                         });

            var taskTeamListDtos = await query.ToListAsync();

            return _taskTeamsExcelExporter.ExportToFile(taskTeamListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTeams)]
        public async Task<PagedResultDto<TaskTeamTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_taskEventRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var taskEventList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TaskTeamTaskEventLookupTableDto>();
            foreach (var taskEvent in taskEventList)
            {
                lookupTableDtoList.Add(new TaskTeamTaskEventLookupTableDto
                {
                    Id = taskEvent.Id,
                    DisplayName = taskEvent.Name?.ToString()
                });
            }

            return new PagedResultDto<TaskTeamTaskEventLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTeams)]
        public async Task<PagedResultDto<TaskTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TaskTeamEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new TaskTeamEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<TaskTeamEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTeams)]
        public async Task<PagedResultDto<TaskTeamContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TaskTeamContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new TaskTeamContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<TaskTeamContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}