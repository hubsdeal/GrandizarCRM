using SoftGrid.CRM;
using SoftGrid.TaskManagement;

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
    [AbpAuthorize(AppPermissions.Pages_BusinessTaskMaps)]
    public class BusinessTaskMapsAppService : SoftGridAppServiceBase, IBusinessTaskMapsAppService
    {
        private readonly IRepository<BusinessTaskMap, long> _businessTaskMapRepository;
        private readonly IBusinessTaskMapsExcelExporter _businessTaskMapsExcelExporter;
        private readonly IRepository<Business, long> _lookup_businessRepository;
        private readonly IRepository<TaskEvent, long> _lookup_taskEventRepository;

        public BusinessTaskMapsAppService(IRepository<BusinessTaskMap, long> businessTaskMapRepository, IBusinessTaskMapsExcelExporter businessTaskMapsExcelExporter, IRepository<Business, long> lookup_businessRepository, IRepository<TaskEvent, long> lookup_taskEventRepository)
        {
            _businessTaskMapRepository = businessTaskMapRepository;
            _businessTaskMapsExcelExporter = businessTaskMapsExcelExporter;
            _lookup_businessRepository = lookup_businessRepository;
            _lookup_taskEventRepository = lookup_taskEventRepository;

        }

        public async Task<PagedResultDto<GetBusinessTaskMapForViewDto>> GetAll(GetAllBusinessTaskMapsInput input)
        {

            var filteredBusinessTaskMaps = _businessTaskMapRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.TaskEventFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter);

            var pagedAndFilteredBusinessTaskMaps = filteredBusinessTaskMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var businessTaskMaps = from o in pagedAndFilteredBusinessTaskMaps
                                   join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   join o2 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o2.Id into j2
                                   from s2 in j2.DefaultIfEmpty()

                                   select new
                                   {

                                       Id = o.Id,
                                       BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                       TaskEventName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                   };

            var totalCount = await filteredBusinessTaskMaps.CountAsync();

            var dbList = await businessTaskMaps.ToListAsync();
            var results = new List<GetBusinessTaskMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBusinessTaskMapForViewDto()
                {
                    BusinessTaskMap = new BusinessTaskMapDto
                    {

                        Id = o.Id,
                    },
                    BusinessName = o.BusinessName,
                    TaskEventName = o.TaskEventName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBusinessTaskMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBusinessTaskMapForViewDto> GetBusinessTaskMapForView(long id)
        {
            var businessTaskMap = await _businessTaskMapRepository.GetAsync(id);

            var output = new GetBusinessTaskMapForViewDto { BusinessTaskMap = ObjectMapper.Map<BusinessTaskMapDto>(businessTaskMap) };

            if (output.BusinessTaskMap.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessTaskMap.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessTaskMap.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.BusinessTaskMap.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessTaskMaps_Edit)]
        public async Task<GetBusinessTaskMapForEditOutput> GetBusinessTaskMapForEdit(EntityDto<long> input)
        {
            var businessTaskMap = await _businessTaskMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBusinessTaskMapForEditOutput { BusinessTaskMap = ObjectMapper.Map<CreateOrEditBusinessTaskMapDto>(businessTaskMap) };

            if (output.BusinessTaskMap.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessTaskMap.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessTaskMap.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.BusinessTaskMap.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessTaskMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BusinessTaskMaps_Create)]
        protected virtual async Task Create(CreateOrEditBusinessTaskMapDto input)
        {
            var businessTaskMap = ObjectMapper.Map<BusinessTaskMap>(input);

            if (AbpSession.TenantId != null)
            {
                businessTaskMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _businessTaskMapRepository.InsertAsync(businessTaskMap);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessTaskMaps_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessTaskMapDto input)
        {
            var businessTaskMap = await _businessTaskMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, businessTaskMap);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessTaskMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _businessTaskMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBusinessTaskMapsToExcel(GetAllBusinessTaskMapsForExcelInput input)
        {

            var filteredBusinessTaskMaps = _businessTaskMapRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.TaskEventFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter);

            var query = (from o in filteredBusinessTaskMaps
                         join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetBusinessTaskMapForViewDto()
                         {
                             BusinessTaskMap = new BusinessTaskMapDto
                             {
                                 Id = o.Id
                             },
                             BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             TaskEventName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var businessTaskMapListDtos = await query.ToListAsync();

            return _businessTaskMapsExcelExporter.ExportToFile(businessTaskMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessTaskMaps)]
        public async Task<PagedResultDto<BusinessTaskMapBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_businessRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var businessList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessTaskMapBusinessLookupTableDto>();
            foreach (var business in businessList)
            {
                lookupTableDtoList.Add(new BusinessTaskMapBusinessLookupTableDto
                {
                    Id = business.Id,
                    DisplayName = business.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessTaskMapBusinessLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessTaskMaps)]
        public async Task<PagedResultDto<BusinessTaskMapTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_taskEventRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var taskEventList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessTaskMapTaskEventLookupTableDto>();
            foreach (var taskEvent in taskEventList)
            {
                lookupTableDtoList.Add(new BusinessTaskMapTaskEventLookupTableDto
                {
                    Id = taskEvent.Id,
                    DisplayName = taskEvent.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessTaskMapTaskEventLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}