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
    [AbpAuthorize(AppPermissions.Pages_JobStatusTypes)]
    public class JobStatusTypesAppService : SoftGridAppServiceBase, IJobStatusTypesAppService
    {
        private readonly IRepository<JobStatusType, long> _jobStatusTypeRepository;
        private readonly IJobStatusTypesExcelExporter _jobStatusTypesExcelExporter;

        public JobStatusTypesAppService(IRepository<JobStatusType, long> jobStatusTypeRepository, IJobStatusTypesExcelExporter jobStatusTypesExcelExporter)
        {
            _jobStatusTypeRepository = jobStatusTypeRepository;
            _jobStatusTypesExcelExporter = jobStatusTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetJobStatusTypeForViewDto>> GetAll(GetAllJobStatusTypesInput input)
        {

            var filteredJobStatusTypes = _jobStatusTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredJobStatusTypes = filteredJobStatusTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var jobStatusTypes = from o in pagedAndFilteredJobStatusTypes
                                 select new
                                 {

                                     o.Name,
                                     Id = o.Id
                                 };

            var totalCount = await filteredJobStatusTypes.CountAsync();

            var dbList = await jobStatusTypes.ToListAsync();
            var results = new List<GetJobStatusTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetJobStatusTypeForViewDto()
                {
                    JobStatusType = new JobStatusTypeDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetJobStatusTypeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetJobStatusTypeForViewDto> GetJobStatusTypeForView(long id)
        {
            var jobStatusType = await _jobStatusTypeRepository.GetAsync(id);

            var output = new GetJobStatusTypeForViewDto { JobStatusType = ObjectMapper.Map<JobStatusTypeDto>(jobStatusType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_JobStatusTypes_Edit)]
        public async Task<GetJobStatusTypeForEditOutput> GetJobStatusTypeForEdit(EntityDto<long> input)
        {
            var jobStatusType = await _jobStatusTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetJobStatusTypeForEditOutput { JobStatusType = ObjectMapper.Map<CreateOrEditJobStatusTypeDto>(jobStatusType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditJobStatusTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_JobStatusTypes_Create)]
        protected virtual async Task Create(CreateOrEditJobStatusTypeDto input)
        {
            var jobStatusType = ObjectMapper.Map<JobStatusType>(input);

            if (AbpSession.TenantId != null)
            {
                jobStatusType.TenantId = (int?)AbpSession.TenantId;
            }

            await _jobStatusTypeRepository.InsertAsync(jobStatusType);

        }

        [AbpAuthorize(AppPermissions.Pages_JobStatusTypes_Edit)]
        protected virtual async Task Update(CreateOrEditJobStatusTypeDto input)
        {
            var jobStatusType = await _jobStatusTypeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, jobStatusType);

        }

        [AbpAuthorize(AppPermissions.Pages_JobStatusTypes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _jobStatusTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetJobStatusTypesToExcel(GetAllJobStatusTypesForExcelInput input)
        {

            var filteredJobStatusTypes = _jobStatusTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredJobStatusTypes
                         select new GetJobStatusTypeForViewDto()
                         {
                             JobStatusType = new JobStatusTypeDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var jobStatusTypeListDtos = await query.ToListAsync();

            return _jobStatusTypesExcelExporter.ExportToFile(jobStatusTypeListDtos);
        }

    }
}