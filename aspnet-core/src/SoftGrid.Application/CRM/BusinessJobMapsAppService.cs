using SoftGrid.CRM;
using SoftGrid.JobManagement;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.CRM.Exporting;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.CRM
{
    [AbpAuthorize(AppPermissions.Pages_BusinessJobMaps)]
    public class BusinessJobMapsAppService : SoftGridAppServiceBase, IBusinessJobMapsAppService
    {
        private readonly IRepository<BusinessJobMap, long> _businessJobMapRepository;
        private readonly IBusinessJobMapsExcelExporter _businessJobMapsExcelExporter;
        private readonly IRepository<Business, long> _lookup_businessRepository;
        private readonly IRepository<Job, long> _lookup_jobRepository;

        public BusinessJobMapsAppService(IRepository<BusinessJobMap, long> businessJobMapRepository, IBusinessJobMapsExcelExporter businessJobMapsExcelExporter, IRepository<Business, long> lookup_businessRepository, IRepository<Job, long> lookup_jobRepository)
        {
            _businessJobMapRepository = businessJobMapRepository;
            _businessJobMapsExcelExporter = businessJobMapsExcelExporter;
            _lookup_businessRepository = lookup_businessRepository;
            _lookup_jobRepository = lookup_jobRepository;

        }

        public async Task<PagedResultDto<GetBusinessJobMapForViewDto>> GetAll(GetAllBusinessJobMapsInput input)
        {

            var filteredBusinessJobMaps = _businessJobMapRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.JobFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobFk != null && e.JobFk.Title == input.JobTitleFilter);

            var pagedAndFilteredBusinessJobMaps = filteredBusinessJobMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var businessJobMaps = from o in pagedAndFilteredBusinessJobMaps
                                  join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_jobRepository.GetAll() on o.JobId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  select new
                                  {

                                      Id = o.Id,
                                      BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                      JobTitle = s2 == null || s2.Title == null ? "" : s2.Title.ToString()
                                  };

            var totalCount = await filteredBusinessJobMaps.CountAsync();

            var dbList = await businessJobMaps.ToListAsync();
            var results = new List<GetBusinessJobMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBusinessJobMapForViewDto()
                {
                    BusinessJobMap = new BusinessJobMapDto
                    {

                        Id = o.Id,
                    },
                    BusinessName = o.BusinessName,
                    JobTitle = o.JobTitle
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBusinessJobMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBusinessJobMapForViewDto> GetBusinessJobMapForView(long id)
        {
            var businessJobMap = await _businessJobMapRepository.GetAsync(id);

            var output = new GetBusinessJobMapForViewDto { BusinessJobMap = ObjectMapper.Map<BusinessJobMapDto>(businessJobMap) };

            if (output.BusinessJobMap.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessJobMap.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessJobMap.JobId != null)
            {
                var _lookupJob = await _lookup_jobRepository.FirstOrDefaultAsync((long)output.BusinessJobMap.JobId);
                output.JobTitle = _lookupJob?.Title?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessJobMaps_Edit)]
        public async Task<GetBusinessJobMapForEditOutput> GetBusinessJobMapForEdit(EntityDto<long> input)
        {
            var businessJobMap = await _businessJobMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBusinessJobMapForEditOutput { BusinessJobMap = ObjectMapper.Map<CreateOrEditBusinessJobMapDto>(businessJobMap) };

            if (output.BusinessJobMap.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessJobMap.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessJobMap.JobId != null)
            {
                var _lookupJob = await _lookup_jobRepository.FirstOrDefaultAsync((long)output.BusinessJobMap.JobId);
                output.JobTitle = _lookupJob?.Title?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessJobMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BusinessJobMaps_Create)]
        protected virtual async Task Create(CreateOrEditBusinessJobMapDto input)
        {
            var businessJobMap = ObjectMapper.Map<BusinessJobMap>(input);

            if (AbpSession.TenantId != null)
            {
                businessJobMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _businessJobMapRepository.InsertAsync(businessJobMap);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessJobMaps_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessJobMapDto input)
        {
            var businessJobMap = await _businessJobMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, businessJobMap);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessJobMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _businessJobMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBusinessJobMapsToExcel(GetAllBusinessJobMapsForExcelInput input)
        {

            var filteredBusinessJobMaps = _businessJobMapRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.JobFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobFk != null && e.JobFk.Title == input.JobTitleFilter);

            var query = (from o in filteredBusinessJobMaps
                         join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_jobRepository.GetAll() on o.JobId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetBusinessJobMapForViewDto()
                         {
                             BusinessJobMap = new BusinessJobMapDto
                             {
                                 Id = o.Id
                             },
                             BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             JobTitle = s2 == null || s2.Title == null ? "" : s2.Title.ToString()
                         });

            var businessJobMapListDtos = await query.ToListAsync();

            return _businessJobMapsExcelExporter.ExportToFile(businessJobMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessJobMaps)]
        public async Task<PagedResultDto<BusinessJobMapBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_businessRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var businessList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessJobMapBusinessLookupTableDto>();
            foreach (var business in businessList)
            {
                lookupTableDtoList.Add(new BusinessJobMapBusinessLookupTableDto
                {
                    Id = business.Id,
                    DisplayName = business.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessJobMapBusinessLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessJobMaps)]
        public async Task<PagedResultDto<BusinessJobMapJobLookupTableDto>> GetAllJobForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_jobRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Title != null && e.Title.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var jobList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessJobMapJobLookupTableDto>();
            foreach (var job in jobList)
            {
                lookupTableDtoList.Add(new BusinessJobMapJobLookupTableDto
                {
                    Id = job.Id,
                    DisplayName = job.Title?.ToString()
                });
            }

            return new PagedResultDto<BusinessJobMapJobLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}