using SoftGrid.SalesLeadManagement;
using SoftGrid.TaskManagement;

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
    [AbpAuthorize(AppPermissions.Pages_LeadTasks)]
    public class LeadTasksAppService : SoftGridAppServiceBase, ILeadTasksAppService
    {
        private readonly IRepository<LeadTask, long> _leadTaskRepository;
        private readonly ILeadTasksExcelExporter _leadTasksExcelExporter;
        private readonly IRepository<Lead, long> _lookup_leadRepository;
        private readonly IRepository<TaskEvent, long> _lookup_taskEventRepository;

        public LeadTasksAppService(IRepository<LeadTask, long> leadTaskRepository, ILeadTasksExcelExporter leadTasksExcelExporter, IRepository<Lead, long> lookup_leadRepository, IRepository<TaskEvent, long> lookup_taskEventRepository)
        {
            _leadTaskRepository = leadTaskRepository;
            _leadTasksExcelExporter = leadTasksExcelExporter;
            _lookup_leadRepository = lookup_leadRepository;
            _lookup_taskEventRepository = lookup_taskEventRepository;

        }

        public async Task<PagedResultDto<GetLeadTaskForViewDto>> GetAll(GetAllLeadTasksInput input)
        {

            var filteredLeadTasks = _leadTaskRepository.GetAll()
                        .Include(e => e.LeadFk)
                        .Include(e => e.TaskEventFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadTitleFilter), e => e.LeadFk != null && e.LeadFk.Title == input.LeadTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter);

            var pagedAndFilteredLeadTasks = filteredLeadTasks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var leadTasks = from o in pagedAndFilteredLeadTasks
                            join o1 in _lookup_leadRepository.GetAll() on o.LeadId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o2 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            select new
                            {

                                Id = o.Id,
                                LeadTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                                TaskEventName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                            };

            var totalCount = await filteredLeadTasks.CountAsync();

            var dbList = await leadTasks.ToListAsync();
            var results = new List<GetLeadTaskForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLeadTaskForViewDto()
                {
                    LeadTask = new LeadTaskDto
                    {

                        Id = o.Id,
                    },
                    LeadTitle = o.LeadTitle,
                    TaskEventName = o.TaskEventName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLeadTaskForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetLeadTaskForViewDto> GetLeadTaskForView(long id)
        {
            var leadTask = await _leadTaskRepository.GetAsync(id);

            var output = new GetLeadTaskForViewDto { LeadTask = ObjectMapper.Map<LeadTaskDto>(leadTask) };

            if (output.LeadTask.LeadId != null)
            {
                var _lookupLead = await _lookup_leadRepository.FirstOrDefaultAsync((long)output.LeadTask.LeadId);
                output.LeadTitle = _lookupLead?.Title?.ToString();
            }

            if (output.LeadTask.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.LeadTask.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_LeadTasks_Edit)]
        public async Task<GetLeadTaskForEditOutput> GetLeadTaskForEdit(EntityDto<long> input)
        {
            var leadTask = await _leadTaskRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLeadTaskForEditOutput { LeadTask = ObjectMapper.Map<CreateOrEditLeadTaskDto>(leadTask) };

            if (output.LeadTask.LeadId != null)
            {
                var _lookupLead = await _lookup_leadRepository.FirstOrDefaultAsync((long)output.LeadTask.LeadId);
                output.LeadTitle = _lookupLead?.Title?.ToString();
            }

            if (output.LeadTask.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.LeadTask.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLeadTaskDto input)
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

        [AbpAuthorize(AppPermissions.Pages_LeadTasks_Create)]
        protected virtual async Task Create(CreateOrEditLeadTaskDto input)
        {
            var leadTask = ObjectMapper.Map<LeadTask>(input);

            if (AbpSession.TenantId != null)
            {
                leadTask.TenantId = (int?)AbpSession.TenantId;
            }

            await _leadTaskRepository.InsertAsync(leadTask);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadTasks_Edit)]
        protected virtual async Task Update(CreateOrEditLeadTaskDto input)
        {
            var leadTask = await _leadTaskRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, leadTask);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadTasks_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _leadTaskRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetLeadTasksToExcel(GetAllLeadTasksForExcelInput input)
        {

            var filteredLeadTasks = _leadTaskRepository.GetAll()
                        .Include(e => e.LeadFk)
                        .Include(e => e.TaskEventFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadTitleFilter), e => e.LeadFk != null && e.LeadFk.Title == input.LeadTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter);

            var query = (from o in filteredLeadTasks
                         join o1 in _lookup_leadRepository.GetAll() on o.LeadId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetLeadTaskForViewDto()
                         {
                             LeadTask = new LeadTaskDto
                             {
                                 Id = o.Id
                             },
                             LeadTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                             TaskEventName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var leadTaskListDtos = await query.ToListAsync();

            return _leadTasksExcelExporter.ExportToFile(leadTaskListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_LeadTasks)]
        public async Task<PagedResultDto<LeadTaskLeadLookupTableDto>> GetAllLeadForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_leadRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Title != null && e.Title.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var leadList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadTaskLeadLookupTableDto>();
            foreach (var lead in leadList)
            {
                lookupTableDtoList.Add(new LeadTaskLeadLookupTableDto
                {
                    Id = lead.Id,
                    DisplayName = lead.Title?.ToString()
                });
            }

            return new PagedResultDto<LeadTaskLeadLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_LeadTasks)]
        public async Task<PagedResultDto<LeadTaskTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_taskEventRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var taskEventList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadTaskTaskEventLookupTableDto>();
            foreach (var taskEvent in taskEventList)
            {
                lookupTableDtoList.Add(new LeadTaskTaskEventLookupTableDto
                {
                    Id = taskEvent.Id,
                    DisplayName = taskEvent.Name?.ToString()
                });
            }

            return new PagedResultDto<LeadTaskTaskEventLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}