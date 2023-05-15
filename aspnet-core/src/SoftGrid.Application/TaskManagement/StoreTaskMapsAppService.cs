using SoftGrid.Shop;
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
    [AbpAuthorize(AppPermissions.Pages_StoreTaskMaps)]
    public class StoreTaskMapsAppService : SoftGridAppServiceBase, IStoreTaskMapsAppService
    {
        private readonly IRepository<StoreTaskMap, long> _storeTaskMapRepository;
        private readonly IStoreTaskMapsExcelExporter _storeTaskMapsExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<TaskEvent, long> _lookup_taskEventRepository;

        public StoreTaskMapsAppService(IRepository<StoreTaskMap, long> storeTaskMapRepository, IStoreTaskMapsExcelExporter storeTaskMapsExcelExporter, IRepository<Store, long> lookup_storeRepository, IRepository<TaskEvent, long> lookup_taskEventRepository)
        {
            _storeTaskMapRepository = storeTaskMapRepository;
            _storeTaskMapsExcelExporter = storeTaskMapsExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_taskEventRepository = lookup_taskEventRepository;

        }

        public async Task<PagedResultDto<GetStoreTaskMapForViewDto>> GetAll(GetAllStoreTaskMapsInput input)
        {

            var filteredStoreTaskMaps = _storeTaskMapRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.TaskEventFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter);

            var pagedAndFilteredStoreTaskMaps = filteredStoreTaskMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeTaskMaps = from o in pagedAndFilteredStoreTaskMaps
                                join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                select new
                                {

                                    Id = o.Id,
                                    StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                    TaskEventName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                };

            var totalCount = await filteredStoreTaskMaps.CountAsync();

            var dbList = await storeTaskMaps.ToListAsync();
            var results = new List<GetStoreTaskMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreTaskMapForViewDto()
                {
                    StoreTaskMap = new StoreTaskMapDto
                    {

                        Id = o.Id,
                    },
                    StoreName = o.StoreName,
                    TaskEventName = o.TaskEventName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreTaskMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreTaskMapForViewDto> GetStoreTaskMapForView(long id)
        {
            var storeTaskMap = await _storeTaskMapRepository.GetAsync(id);

            var output = new GetStoreTaskMapForViewDto { StoreTaskMap = ObjectMapper.Map<StoreTaskMapDto>(storeTaskMap) };

            if (output.StoreTaskMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreTaskMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreTaskMap.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.StoreTaskMap.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreTaskMaps_Edit)]
        public async Task<GetStoreTaskMapForEditOutput> GetStoreTaskMapForEdit(EntityDto<long> input)
        {
            var storeTaskMap = await _storeTaskMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreTaskMapForEditOutput { StoreTaskMap = ObjectMapper.Map<CreateOrEditStoreTaskMapDto>(storeTaskMap) };

            if (output.StoreTaskMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreTaskMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreTaskMap.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.StoreTaskMap.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreTaskMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreTaskMaps_Create)]
        protected virtual async Task Create(CreateOrEditStoreTaskMapDto input)
        {
            var storeTaskMap = ObjectMapper.Map<StoreTaskMap>(input);

            if (AbpSession.TenantId != null)
            {
                storeTaskMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeTaskMapRepository.InsertAsync(storeTaskMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreTaskMaps_Edit)]
        protected virtual async Task Update(CreateOrEditStoreTaskMapDto input)
        {
            var storeTaskMap = await _storeTaskMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeTaskMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreTaskMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeTaskMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreTaskMapsToExcel(GetAllStoreTaskMapsForExcelInput input)
        {

            var filteredStoreTaskMaps = _storeTaskMapRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.TaskEventFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter);

            var query = (from o in filteredStoreTaskMaps
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStoreTaskMapForViewDto()
                         {
                             StoreTaskMap = new StoreTaskMapDto
                             {
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             TaskEventName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var storeTaskMapListDtos = await query.ToListAsync();

            return _storeTaskMapsExcelExporter.ExportToFile(storeTaskMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreTaskMaps)]
        public async Task<PagedResultDto<StoreTaskMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreTaskMapStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreTaskMapStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreTaskMapStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreTaskMaps)]
        public async Task<PagedResultDto<StoreTaskMapTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_taskEventRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var taskEventList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreTaskMapTaskEventLookupTableDto>();
            foreach (var taskEvent in taskEventList)
            {
                lookupTableDtoList.Add(new StoreTaskMapTaskEventLookupTableDto
                {
                    Id = taskEvent.Id,
                    DisplayName = taskEvent.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreTaskMapTaskEventLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}