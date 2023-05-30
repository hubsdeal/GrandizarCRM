using SoftGrid.WidgetManagement;
using SoftGrid.Shop;

using SoftGrid.WidgetManagement.Enums;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.WidgetManagement.Exporting;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.WidgetManagement
{
    [AbpAuthorize(AppPermissions.Pages_StoreWidgetMaps)]
    public class StoreWidgetMapsAppService : SoftGridAppServiceBase, IStoreWidgetMapsAppService
    {
        private readonly IRepository<StoreWidgetMap, long> _storeWidgetMapRepository;
        private readonly IStoreWidgetMapsExcelExporter _storeWidgetMapsExcelExporter;
        private readonly IRepository<MasterWidget, long> _lookup_masterWidgetRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;

        public StoreWidgetMapsAppService(IRepository<StoreWidgetMap, long> storeWidgetMapRepository, IStoreWidgetMapsExcelExporter storeWidgetMapsExcelExporter, IRepository<MasterWidget, long> lookup_masterWidgetRepository, IRepository<Store, long> lookup_storeRepository)
        {
            _storeWidgetMapRepository = storeWidgetMapRepository;
            _storeWidgetMapsExcelExporter = storeWidgetMapsExcelExporter;
            _lookup_masterWidgetRepository = lookup_masterWidgetRepository;
            _lookup_storeRepository = lookup_storeRepository;

        }

        public async Task<PagedResultDto<GetStoreWidgetMapForViewDto>> GetAll(GetAllStoreWidgetMapsInput input)
        {
            var widgetTypeIdFilter = input.WidgetTypeIdFilter.HasValue
                        ? (WidgetType)input.WidgetTypeIdFilter
                        : default;

            var filteredStoreWidgetMaps = _storeWidgetMapRepository.GetAll()
                        .Include(e => e.MasterWidgetFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomName.Contains(input.Filter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.WidgetTypeIdFilter.HasValue && input.WidgetTypeIdFilter > -1, e => e.WidgetTypeId == widgetTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomNameFilter), e => e.CustomName.Contains(input.CustomNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterWidgetNameFilter), e => e.MasterWidgetFk != null && e.MasterWidgetFk.Name == input.MasterWidgetNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var pagedAndFilteredStoreWidgetMaps = filteredStoreWidgetMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeWidgetMaps = from o in pagedAndFilteredStoreWidgetMaps
                                  join o1 in _lookup_masterWidgetRepository.GetAll() on o.MasterWidgetId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_storeRepository.GetAll() on o.StoreId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  select new
                                  {

                                      o.DisplaySequence,
                                      o.WidgetTypeId,
                                      o.CustomName,
                                      Id = o.Id,
                                      MasterWidgetName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                      StoreName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                  };

            var totalCount = await filteredStoreWidgetMaps.CountAsync();

            var dbList = await storeWidgetMaps.ToListAsync();
            var results = new List<GetStoreWidgetMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreWidgetMapForViewDto()
                {
                    StoreWidgetMap = new StoreWidgetMapDto
                    {

                        DisplaySequence = o.DisplaySequence,
                        WidgetTypeId = o.WidgetTypeId,
                        CustomName = o.CustomName,
                        Id = o.Id,
                    },
                    MasterWidgetName = o.MasterWidgetName,
                    StoreName = o.StoreName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreWidgetMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreWidgetMapForViewDto> GetStoreWidgetMapForView(long id)
        {
            var storeWidgetMap = await _storeWidgetMapRepository.GetAsync(id);

            var output = new GetStoreWidgetMapForViewDto { StoreWidgetMap = ObjectMapper.Map<StoreWidgetMapDto>(storeWidgetMap) };

            if (output.StoreWidgetMap.MasterWidgetId != null)
            {
                var _lookupMasterWidget = await _lookup_masterWidgetRepository.FirstOrDefaultAsync((long)output.StoreWidgetMap.MasterWidgetId);
                output.MasterWidgetName = _lookupMasterWidget?.Name?.ToString();
            }

            if (output.StoreWidgetMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreWidgetMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetMaps_Edit)]
        public async Task<GetStoreWidgetMapForEditOutput> GetStoreWidgetMapForEdit(EntityDto<long> input)
        {
            var storeWidgetMap = await _storeWidgetMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreWidgetMapForEditOutput { StoreWidgetMap = ObjectMapper.Map<CreateOrEditStoreWidgetMapDto>(storeWidgetMap) };

            if (output.StoreWidgetMap.MasterWidgetId != null)
            {
                var _lookupMasterWidget = await _lookup_masterWidgetRepository.FirstOrDefaultAsync((long)output.StoreWidgetMap.MasterWidgetId);
                output.MasterWidgetName = _lookupMasterWidget?.Name?.ToString();
            }

            if (output.StoreWidgetMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreWidgetMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreWidgetMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetMaps_Create)]
        protected virtual async Task Create(CreateOrEditStoreWidgetMapDto input)
        {
            var storeWidgetMap = ObjectMapper.Map<StoreWidgetMap>(input);

            if (AbpSession.TenantId != null)
            {
                storeWidgetMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeWidgetMapRepository.InsertAsync(storeWidgetMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetMaps_Edit)]
        protected virtual async Task Update(CreateOrEditStoreWidgetMapDto input)
        {
            var storeWidgetMap = await _storeWidgetMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeWidgetMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeWidgetMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreWidgetMapsToExcel(GetAllStoreWidgetMapsForExcelInput input)
        {
            var widgetTypeIdFilter = input.WidgetTypeIdFilter.HasValue
                        ? (WidgetType)input.WidgetTypeIdFilter
                        : default;

            var filteredStoreWidgetMaps = _storeWidgetMapRepository.GetAll()
                        .Include(e => e.MasterWidgetFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomName.Contains(input.Filter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.WidgetTypeIdFilter.HasValue && input.WidgetTypeIdFilter > -1, e => e.WidgetTypeId == widgetTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomNameFilter), e => e.CustomName.Contains(input.CustomNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterWidgetNameFilter), e => e.MasterWidgetFk != null && e.MasterWidgetFk.Name == input.MasterWidgetNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var query = (from o in filteredStoreWidgetMaps
                         join o1 in _lookup_masterWidgetRepository.GetAll() on o.MasterWidgetId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_storeRepository.GetAll() on o.StoreId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStoreWidgetMapForViewDto()
                         {
                             StoreWidgetMap = new StoreWidgetMapDto
                             {
                                 DisplaySequence = o.DisplaySequence,
                                 WidgetTypeId = o.WidgetTypeId,
                                 CustomName = o.CustomName,
                                 Id = o.Id
                             },
                             MasterWidgetName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             StoreName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var storeWidgetMapListDtos = await query.ToListAsync();

            return _storeWidgetMapsExcelExporter.ExportToFile(storeWidgetMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetMaps)]
        public async Task<PagedResultDto<StoreWidgetMapMasterWidgetLookupTableDto>> GetAllMasterWidgetForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterWidgetRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterWidgetList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreWidgetMapMasterWidgetLookupTableDto>();
            foreach (var masterWidget in masterWidgetList)
            {
                lookupTableDtoList.Add(new StoreWidgetMapMasterWidgetLookupTableDto
                {
                    Id = masterWidget.Id,
                    DisplayName = masterWidget.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreWidgetMapMasterWidgetLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetMaps)]
        public async Task<PagedResultDto<StoreWidgetMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreWidgetMapStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreWidgetMapStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreWidgetMapStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}