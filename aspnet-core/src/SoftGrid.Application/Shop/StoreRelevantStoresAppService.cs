using SoftGrid.Shop;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.Shop.Exporting;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.Shop
{
    [AbpAuthorize(AppPermissions.Pages_StoreRelevantStores)]
    public class StoreRelevantStoresAppService : SoftGridAppServiceBase, IStoreRelevantStoresAppService
    {
        private readonly IRepository<StoreRelevantStore, long> _storeRelevantStoreRepository;
        private readonly IStoreRelevantStoresExcelExporter _storeRelevantStoresExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;

        public StoreRelevantStoresAppService(IRepository<StoreRelevantStore, long> storeRelevantStoreRepository, IStoreRelevantStoresExcelExporter storeRelevantStoresExcelExporter, IRepository<Store, long> lookup_storeRepository)
        {
            _storeRelevantStoreRepository = storeRelevantStoreRepository;
            _storeRelevantStoresExcelExporter = storeRelevantStoresExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;

        }

        public async Task<PagedResultDto<GetStoreRelevantStoreForViewDto>> GetAll(GetAllStoreRelevantStoresInput input)
        {

            var filteredStoreRelevantStores = _storeRelevantStoreRepository.GetAll()
                        .Include(e => e.PrimaryStoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinRelevantStoreIdFilter != null, e => e.RelevantStoreId >= input.MinRelevantStoreIdFilter)
                        .WhereIf(input.MaxRelevantStoreIdFilter != null, e => e.RelevantStoreId <= input.MaxRelevantStoreIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.PrimaryStoreFk != null && e.PrimaryStoreFk.Name == input.StoreNameFilter);

            var pagedAndFilteredStoreRelevantStores = filteredStoreRelevantStores
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeRelevantStores = from o in pagedAndFilteredStoreRelevantStores
                                      join o1 in _lookup_storeRepository.GetAll() on o.PrimaryStoreId equals o1.Id into j1
                                      from s1 in j1.DefaultIfEmpty()

                                      select new
                                      {

                                          o.RelevantStoreId,
                                          Id = o.Id,
                                          StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                      };

            var totalCount = await filteredStoreRelevantStores.CountAsync();

            var dbList = await storeRelevantStores.ToListAsync();
            var results = new List<GetStoreRelevantStoreForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreRelevantStoreForViewDto()
                {
                    StoreRelevantStore = new StoreRelevantStoreDto
                    {

                        RelevantStoreId = o.RelevantStoreId,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreRelevantStoreForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreRelevantStoreForViewDto> GetStoreRelevantStoreForView(long id)
        {
            var storeRelevantStore = await _storeRelevantStoreRepository.GetAsync(id);

            var output = new GetStoreRelevantStoreForViewDto { StoreRelevantStore = ObjectMapper.Map<StoreRelevantStoreDto>(storeRelevantStore) };

            if (output.StoreRelevantStore.PrimaryStoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreRelevantStore.PrimaryStoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreRelevantStores_Edit)]
        public async Task<GetStoreRelevantStoreForEditOutput> GetStoreRelevantStoreForEdit(EntityDto<long> input)
        {
            var storeRelevantStore = await _storeRelevantStoreRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreRelevantStoreForEditOutput { StoreRelevantStore = ObjectMapper.Map<CreateOrEditStoreRelevantStoreDto>(storeRelevantStore) };

            if (output.StoreRelevantStore.PrimaryStoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreRelevantStore.PrimaryStoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreRelevantStoreDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreRelevantStores_Create)]
        protected virtual async Task Create(CreateOrEditStoreRelevantStoreDto input)
        {
            var storeRelevantStore = ObjectMapper.Map<StoreRelevantStore>(input);

            if (AbpSession.TenantId != null)
            {
                storeRelevantStore.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeRelevantStoreRepository.InsertAsync(storeRelevantStore);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreRelevantStores_Edit)]
        protected virtual async Task Update(CreateOrEditStoreRelevantStoreDto input)
        {
            var storeRelevantStore = await _storeRelevantStoreRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeRelevantStore);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreRelevantStores_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeRelevantStoreRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreRelevantStoresToExcel(GetAllStoreRelevantStoresForExcelInput input)
        {

            var filteredStoreRelevantStores = _storeRelevantStoreRepository.GetAll()
                        .Include(e => e.PrimaryStoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinRelevantStoreIdFilter != null, e => e.RelevantStoreId >= input.MinRelevantStoreIdFilter)
                        .WhereIf(input.MaxRelevantStoreIdFilter != null, e => e.RelevantStoreId <= input.MaxRelevantStoreIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.PrimaryStoreFk != null && e.PrimaryStoreFk.Name == input.StoreNameFilter);

            var query = (from o in filteredStoreRelevantStores
                         join o1 in _lookup_storeRepository.GetAll() on o.PrimaryStoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetStoreRelevantStoreForViewDto()
                         {
                             StoreRelevantStore = new StoreRelevantStoreDto
                             {
                                 RelevantStoreId = o.RelevantStoreId,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var storeRelevantStoreListDtos = await query.ToListAsync();

            return _storeRelevantStoresExcelExporter.ExportToFile(storeRelevantStoreListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreRelevantStores)]
        public async Task<PagedResultDto<StoreRelevantStoreStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreRelevantStoreStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreRelevantStoreStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreRelevantStoreStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}