using SoftGrid.WidgetManagement;
using SoftGrid.Shop;

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
    [AbpAuthorize(AppPermissions.Pages_StoreThemeMaps)]
    public class StoreThemeMapsAppService : SoftGridAppServiceBase, IStoreThemeMapsAppService
    {
        private readonly IRepository<StoreThemeMap, long> _storeThemeMapRepository;
        private readonly IStoreThemeMapsExcelExporter _storeThemeMapsExcelExporter;
        private readonly IRepository<StoreMasterTheme, long> _lookup_storeMasterThemeRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;

        public StoreThemeMapsAppService(IRepository<StoreThemeMap, long> storeThemeMapRepository, IStoreThemeMapsExcelExporter storeThemeMapsExcelExporter, IRepository<StoreMasterTheme, long> lookup_storeMasterThemeRepository, IRepository<Store, long> lookup_storeRepository)
        {
            _storeThemeMapRepository = storeThemeMapRepository;
            _storeThemeMapsExcelExporter = storeThemeMapsExcelExporter;
            _lookup_storeMasterThemeRepository = lookup_storeMasterThemeRepository;
            _lookup_storeRepository = lookup_storeRepository;

        }

        public async Task<PagedResultDto<GetStoreThemeMapForViewDto>> GetAll(GetAllStoreThemeMapsInput input)
        {

            var filteredStoreThemeMaps = _storeThemeMapRepository.GetAll()
                        .Include(e => e.StoreMasterThemeFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreMasterThemeNameFilter), e => e.StoreMasterThemeFk != null && e.StoreMasterThemeFk.Name == input.StoreMasterThemeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var pagedAndFilteredStoreThemeMaps = filteredStoreThemeMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeThemeMaps = from o in pagedAndFilteredStoreThemeMaps
                                 join o1 in _lookup_storeMasterThemeRepository.GetAll() on o.StoreMasterThemeId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 join o2 in _lookup_storeRepository.GetAll() on o.StoreId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()

                                 select new
                                 {

                                     o.Active,
                                     Id = o.Id,
                                     StoreMasterThemeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                     StoreName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                 };

            var totalCount = await filteredStoreThemeMaps.CountAsync();

            var dbList = await storeThemeMaps.ToListAsync();
            var results = new List<GetStoreThemeMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreThemeMapForViewDto()
                {
                    StoreThemeMap = new StoreThemeMapDto
                    {

                        Active = o.Active,
                        Id = o.Id,
                    },
                    StoreMasterThemeName = o.StoreMasterThemeName,
                    StoreName = o.StoreName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreThemeMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreThemeMapForViewDto> GetStoreThemeMapForView(long id)
        {
            var storeThemeMap = await _storeThemeMapRepository.GetAsync(id);

            var output = new GetStoreThemeMapForViewDto { StoreThemeMap = ObjectMapper.Map<StoreThemeMapDto>(storeThemeMap) };

            if (output.StoreThemeMap.StoreMasterThemeId != null)
            {
                var _lookupStoreMasterTheme = await _lookup_storeMasterThemeRepository.FirstOrDefaultAsync((long)output.StoreThemeMap.StoreMasterThemeId);
                output.StoreMasterThemeName = _lookupStoreMasterTheme?.Name?.ToString();
            }

            if (output.StoreThemeMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreThemeMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreThemeMaps_Edit)]
        public async Task<GetStoreThemeMapForEditOutput> GetStoreThemeMapForEdit(EntityDto<long> input)
        {
            var storeThemeMap = await _storeThemeMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreThemeMapForEditOutput { StoreThemeMap = ObjectMapper.Map<CreateOrEditStoreThemeMapDto>(storeThemeMap) };

            if (output.StoreThemeMap.StoreMasterThemeId != null)
            {
                var _lookupStoreMasterTheme = await _lookup_storeMasterThemeRepository.FirstOrDefaultAsync((long)output.StoreThemeMap.StoreMasterThemeId);
                output.StoreMasterThemeName = _lookupStoreMasterTheme?.Name?.ToString();
            }

            if (output.StoreThemeMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreThemeMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreThemeMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreThemeMaps_Create)]
        protected virtual async Task Create(CreateOrEditStoreThemeMapDto input)
        {
            var storeThemeMap = ObjectMapper.Map<StoreThemeMap>(input);

            if (AbpSession.TenantId != null)
            {
                storeThemeMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeThemeMapRepository.InsertAsync(storeThemeMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreThemeMaps_Edit)]
        protected virtual async Task Update(CreateOrEditStoreThemeMapDto input)
        {
            var storeThemeMap = await _storeThemeMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeThemeMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreThemeMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeThemeMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreThemeMapsToExcel(GetAllStoreThemeMapsForExcelInput input)
        {

            var filteredStoreThemeMaps = _storeThemeMapRepository.GetAll()
                        .Include(e => e.StoreMasterThemeFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreMasterThemeNameFilter), e => e.StoreMasterThemeFk != null && e.StoreMasterThemeFk.Name == input.StoreMasterThemeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var query = (from o in filteredStoreThemeMaps
                         join o1 in _lookup_storeMasterThemeRepository.GetAll() on o.StoreMasterThemeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_storeRepository.GetAll() on o.StoreId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStoreThemeMapForViewDto()
                         {
                             StoreThemeMap = new StoreThemeMapDto
                             {
                                 Active = o.Active,
                                 Id = o.Id
                             },
                             StoreMasterThemeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             StoreName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var storeThemeMapListDtos = await query.ToListAsync();

            return _storeThemeMapsExcelExporter.ExportToFile(storeThemeMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreThemeMaps)]
        public async Task<PagedResultDto<StoreThemeMapStoreMasterThemeLookupTableDto>> GetAllStoreMasterThemeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeMasterThemeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeMasterThemeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreThemeMapStoreMasterThemeLookupTableDto>();
            foreach (var storeMasterTheme in storeMasterThemeList)
            {
                lookupTableDtoList.Add(new StoreThemeMapStoreMasterThemeLookupTableDto
                {
                    Id = storeMasterTheme.Id,
                    DisplayName = storeMasterTheme.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreThemeMapStoreMasterThemeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreThemeMaps)]
        public async Task<PagedResultDto<StoreThemeMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreThemeMapStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreThemeMapStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreThemeMapStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}