using SoftGrid.Territory;
using SoftGrid.Shop;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.Territory.Exporting;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.Territory
{
    [AbpAuthorize(AppPermissions.Pages_HubStores)]
    public class HubStoresAppService : SoftGridAppServiceBase, IHubStoresAppService
    {
        private readonly IRepository<HubStore, long> _hubStoreRepository;
        private readonly IHubStoresExcelExporter _hubStoresExcelExporter;
        private readonly IRepository<Hub, long> _lookup_hubRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;

        public HubStoresAppService(IRepository<HubStore, long> hubStoreRepository, IHubStoresExcelExporter hubStoresExcelExporter, IRepository<Hub, long> lookup_hubRepository, IRepository<Store, long> lookup_storeRepository)
        {
            _hubStoreRepository = hubStoreRepository;
            _hubStoresExcelExporter = hubStoresExcelExporter;
            _lookup_hubRepository = lookup_hubRepository;
            _lookup_storeRepository = lookup_storeRepository;

        }

        public async Task<PagedResultDto<GetHubStoreForViewDto>> GetAll(GetAllHubStoresInput input)
        {

            var filteredHubStores = _hubStoreRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var pagedAndFilteredHubStores = filteredHubStores
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hubStores = from o in pagedAndFilteredHubStores
                            join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o2 in _lookup_storeRepository.GetAll() on o.StoreId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            select new
                            {

                                o.Published,
                                o.DisplaySequence,
                                Id = o.Id,
                                HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                StoreName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                            };

            var totalCount = await filteredHubStores.CountAsync();

            var dbList = await hubStores.ToListAsync();
            var results = new List<GetHubStoreForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHubStoreForViewDto()
                {
                    HubStore = new HubStoreDto
                    {

                        Published = o.Published,
                        DisplaySequence = o.DisplaySequence,
                        Id = o.Id,
                    },
                    HubName = o.HubName,
                    StoreName = o.StoreName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHubStoreForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetHubStoreForViewDto> GetHubStoreForView(long id)
        {
            var hubStore = await _hubStoreRepository.GetAsync(id);

            var output = new GetHubStoreForViewDto { HubStore = ObjectMapper.Map<HubStoreDto>(hubStore) };

            if (output.HubStore.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubStore.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubStore.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.HubStore.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HubStores_Edit)]
        public async Task<GetHubStoreForEditOutput> GetHubStoreForEdit(EntityDto<long> input)
        {
            var hubStore = await _hubStoreRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHubStoreForEditOutput { HubStore = ObjectMapper.Map<CreateOrEditHubStoreDto>(hubStore) };

            if (output.HubStore.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubStore.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubStore.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.HubStore.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHubStoreDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HubStores_Create)]
        protected virtual async Task Create(CreateOrEditHubStoreDto input)
        {
            var hubStore = ObjectMapper.Map<HubStore>(input);

            if (AbpSession.TenantId != null)
            {
                hubStore.TenantId = (int?)AbpSession.TenantId;
            }

            await _hubStoreRepository.InsertAsync(hubStore);

        }

        [AbpAuthorize(AppPermissions.Pages_HubStores_Edit)]
        protected virtual async Task Update(CreateOrEditHubStoreDto input)
        {
            var hubStore = await _hubStoreRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, hubStore);

        }

        [AbpAuthorize(AppPermissions.Pages_HubStores_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _hubStoreRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHubStoresToExcel(GetAllHubStoresForExcelInput input)
        {

            var filteredHubStores = _hubStoreRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var query = (from o in filteredHubStores
                         join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_storeRepository.GetAll() on o.StoreId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetHubStoreForViewDto()
                         {
                             HubStore = new HubStoreDto
                             {
                                 Published = o.Published,
                                 DisplaySequence = o.DisplaySequence,
                                 Id = o.Id
                             },
                             HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             StoreName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var hubStoreListDtos = await query.ToListAsync();

            return _hubStoresExcelExporter.ExportToFile(hubStoreListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_HubStores)]
        public async Task<PagedResultDto<HubStoreHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_hubRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var hubList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubStoreHubLookupTableDto>();
            foreach (var hub in hubList)
            {
                lookupTableDtoList.Add(new HubStoreHubLookupTableDto
                {
                    Id = hub.Id,
                    DisplayName = hub.Name?.ToString()
                });
            }

            return new PagedResultDto<HubStoreHubLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubStores)]
        public async Task<PagedResultDto<HubStoreStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubStoreStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new HubStoreStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<HubStoreStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}