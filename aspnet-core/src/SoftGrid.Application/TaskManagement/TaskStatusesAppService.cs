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
    [AbpAuthorize(AppPermissions.Pages_TaskStatuses)]
    public class TaskStatusesAppService : SoftGridAppServiceBase, ITaskStatusesAppService
    {
        private readonly IRepository<TaskStatus, long> _taskStatusRepository;
        private readonly ITaskStatusesExcelExporter _taskStatusesExcelExporter;

        public TaskStatusesAppService(IRepository<TaskStatus, long> taskStatusRepository, ITaskStatusesExcelExporter taskStatusesExcelExporter)
        {
            _taskStatusRepository = taskStatusRepository;
            _taskStatusesExcelExporter = taskStatusesExcelExporter;

        }

        public async Task<PagedResultDto<GetTaskStatusForViewDto>> GetAll(GetAllTaskStatusesInput input)
        {

            var filteredTaskStatuses = _taskStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredTaskStatuses = filteredTaskStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var taskStatuses = from o in pagedAndFilteredTaskStatuses
                               select new
                               {

                                   o.Name,
                                   Id = o.Id
                               };

            var totalCount = await filteredTaskStatuses.CountAsync();

            var dbList = await taskStatuses.ToListAsync();
            var results = new List<GetTaskStatusForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTaskStatusForViewDto()
                {
                    TaskStatus = new TaskStatusDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTaskStatusForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetTaskStatusForViewDto> GetTaskStatusForView(long id)
        {
            var taskStatus = await _taskStatusRepository.GetAsync(id);

            var output = new GetTaskStatusForViewDto { TaskStatus = ObjectMapper.Map<TaskStatusDto>(taskStatus) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TaskStatuses_Edit)]
        public async Task<GetTaskStatusForEditOutput> GetTaskStatusForEdit(EntityDto<long> input)
        {
            var taskStatus = await _taskStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTaskStatusForEditOutput { TaskStatus = ObjectMapper.Map<CreateOrEditTaskStatusDto>(taskStatus) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTaskStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TaskStatuses_Create)]
        protected virtual async Task Create(CreateOrEditTaskStatusDto input)
        {
            var taskStatus = ObjectMapper.Map<TaskStatus>(input);

            if (AbpSession.TenantId != null)
            {
                taskStatus.TenantId = (int?)AbpSession.TenantId;
            }

            await _taskStatusRepository.InsertAsync(taskStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskStatuses_Edit)]
        protected virtual async Task Update(CreateOrEditTaskStatusDto input)
        {
            var taskStatus = await _taskStatusRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, taskStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskStatuses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _taskStatusRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTaskStatusesToExcel(GetAllTaskStatusesForExcelInput input)
        {

            var filteredTaskStatuses = _taskStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredTaskStatuses
                         select new GetTaskStatusForViewDto()
                         {
                             TaskStatus = new TaskStatusDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var taskStatusListDtos = await query.ToListAsync();

            return _taskStatusesExcelExporter.ExportToFile(taskStatusListDtos);
        }

    }
}