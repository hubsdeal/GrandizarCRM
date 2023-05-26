using SoftGrid.JobManagement;
using SoftGrid.TaskManagement;

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
    [AbpAuthorize(AppPermissions.Pages_JobTaskMaps)]
    public class JobTaskMapsAppService : SoftGridAppServiceBase, IJobTaskMapsAppService
    {
        private readonly IRepository<JobTaskMap, long> _jobTaskMapRepository;
        private readonly IJobTaskMapsExcelExporter _jobTaskMapsExcelExporter;
        private readonly IRepository<Job, long> _lookup_jobRepository;
        private readonly IRepository<TaskEvent, long> _lookup_taskEventRepository;

        public JobTaskMapsAppService(IRepository<JobTaskMap, long> jobTaskMapRepository, IJobTaskMapsExcelExporter jobTaskMapsExcelExporter, IRepository<Job, long> lookup_jobRepository, IRepository<TaskEvent, long> lookup_taskEventRepository)
        {
            _jobTaskMapRepository = jobTaskMapRepository;
            _jobTaskMapsExcelExporter = jobTaskMapsExcelExporter;
            _lookup_jobRepository = lookup_jobRepository;
            _lookup_taskEventRepository = lookup_taskEventRepository;

        }

        public async Task<PagedResultDto<GetJobTaskMapForViewDto>> GetAll(GetAllJobTaskMapsInput input)
        {

            var filteredJobTaskMaps = _jobTaskMapRepository.GetAll()
                        .Include(e => e.JobFk)
                        .Include(e => e.TaskEventFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobFk != null && e.JobFk.Title == input.JobTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter);

            var pagedAndFilteredJobTaskMaps = filteredJobTaskMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var jobTaskMaps = from o in pagedAndFilteredJobTaskMaps
                              join o1 in _lookup_jobRepository.GetAll() on o.JobId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              join o2 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o2.Id into j2
                              from s2 in j2.DefaultIfEmpty()

                              select new
                              {

                                  Id = o.Id,
                                  JobTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                                  TaskEventName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                              };

            var totalCount = await filteredJobTaskMaps.CountAsync();

            var dbList = await jobTaskMaps.ToListAsync();
            var results = new List<GetJobTaskMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetJobTaskMapForViewDto()
                {
                    JobTaskMap = new JobTaskMapDto
                    {

                        Id = o.Id,
                    },
                    JobTitle = o.JobTitle,
                    TaskEventName = o.TaskEventName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetJobTaskMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetJobTaskMapForViewDto> GetJobTaskMapForView(long id)
        {
            var jobTaskMap = await _jobTaskMapRepository.GetAsync(id);

            var output = new GetJobTaskMapForViewDto { JobTaskMap = ObjectMapper.Map<JobTaskMapDto>(jobTaskMap) };

            if (output.JobTaskMap.JobId != null)
            {
                var _lookupJob = await _lookup_jobRepository.FirstOrDefaultAsync((long)output.JobTaskMap.JobId);
                output.JobTitle = _lookupJob?.Title?.ToString();
            }

            if (output.JobTaskMap.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.JobTaskMap.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_JobTaskMaps_Edit)]
        public async Task<GetJobTaskMapForEditOutput> GetJobTaskMapForEdit(EntityDto<long> input)
        {
            var jobTaskMap = await _jobTaskMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetJobTaskMapForEditOutput { JobTaskMap = ObjectMapper.Map<CreateOrEditJobTaskMapDto>(jobTaskMap) };

            if (output.JobTaskMap.JobId != null)
            {
                var _lookupJob = await _lookup_jobRepository.FirstOrDefaultAsync((long)output.JobTaskMap.JobId);
                output.JobTitle = _lookupJob?.Title?.ToString();
            }

            if (output.JobTaskMap.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.JobTaskMap.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditJobTaskMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_JobTaskMaps_Create)]
        protected virtual async Task Create(CreateOrEditJobTaskMapDto input)
        {
            var jobTaskMap = ObjectMapper.Map<JobTaskMap>(input);

            if (AbpSession.TenantId != null)
            {
                jobTaskMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _jobTaskMapRepository.InsertAsync(jobTaskMap);

        }

        [AbpAuthorize(AppPermissions.Pages_JobTaskMaps_Edit)]
        protected virtual async Task Update(CreateOrEditJobTaskMapDto input)
        {
            var jobTaskMap = await _jobTaskMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, jobTaskMap);

        }

        [AbpAuthorize(AppPermissions.Pages_JobTaskMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _jobTaskMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetJobTaskMapsToExcel(GetAllJobTaskMapsForExcelInput input)
        {

            var filteredJobTaskMaps = _jobTaskMapRepository.GetAll()
                        .Include(e => e.JobFk)
                        .Include(e => e.TaskEventFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobFk != null && e.JobFk.Title == input.JobTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter);

            var query = (from o in filteredJobTaskMaps
                         join o1 in _lookup_jobRepository.GetAll() on o.JobId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetJobTaskMapForViewDto()
                         {
                             JobTaskMap = new JobTaskMapDto
                             {
                                 Id = o.Id
                             },
                             JobTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                             TaskEventName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var jobTaskMapListDtos = await query.ToListAsync();

            return _jobTaskMapsExcelExporter.ExportToFile(jobTaskMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_JobTaskMaps)]
        public async Task<PagedResultDto<JobTaskMapJobLookupTableDto>> GetAllJobForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_jobRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Title != null && e.Title.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var jobList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobTaskMapJobLookupTableDto>();
            foreach (var job in jobList)
            {
                lookupTableDtoList.Add(new JobTaskMapJobLookupTableDto
                {
                    Id = job.Id,
                    DisplayName = job.Title?.ToString()
                });
            }

            return new PagedResultDto<JobTaskMapJobLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_JobTaskMaps)]
        public async Task<PagedResultDto<JobTaskMapTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_taskEventRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var taskEventList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobTaskMapTaskEventLookupTableDto>();
            foreach (var taskEvent in taskEventList)
            {
                lookupTableDtoList.Add(new JobTaskMapTaskEventLookupTableDto
                {
                    Id = taskEvent.Id,
                    DisplayName = taskEvent.Name?.ToString()
                });
            }

            return new PagedResultDto<JobTaskMapTaskEventLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}