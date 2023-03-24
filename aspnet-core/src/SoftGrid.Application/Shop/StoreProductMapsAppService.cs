using SoftGrid.Shop;
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
    [AbpAuthorize(AppPermissions.Pages_StoreProductMaps)]
    public class StoreProductMapsAppService : SoftGridAppServiceBase, IStoreProductMapsAppService
    {
        private readonly IRepository<StoreProductMap, long> _storeProductMapRepository;
        private readonly IStoreProductMapsExcelExporter _storeProductMapsExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;

        public StoreProductMapsAppService(IRepository<StoreProductMap, long> storeProductMapRepository, IStoreProductMapsExcelExporter storeProductMapsExcelExporter, IRepository<Store, long> lookup_storeRepository, IRepository<Product, long> lookup_productRepository)
        {
            _storeProductMapRepository = storeProductMapRepository;
            _storeProductMapsExcelExporter = storeProductMapsExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_productRepository = lookup_productRepository;

        }

        public async Task<PagedResultDto<GetStoreProductMapForViewDto>> GetAll(GetAllStoreProductMapsInput input)
        {

            var filteredStoreProductMaps = _storeProductMapRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var pagedAndFilteredStoreProductMaps = filteredStoreProductMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeProductMaps = from o in pagedAndFilteredStoreProductMaps
                                   join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                                   from s2 in j2.DefaultIfEmpty()

                                   select new
                                   {

                                       o.Published,
                                       o.DisplaySequence,
                                       Id = o.Id,
                                       StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                       ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                   };

            var totalCount = await filteredStoreProductMaps.CountAsync();

            var dbList = await storeProductMaps.ToListAsync();
            var results = new List<GetStoreProductMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreProductMapForViewDto()
                {
                    StoreProductMap = new StoreProductMapDto
                    {

                        Published = o.Published,
                        DisplaySequence = o.DisplaySequence,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName,
                    ProductName = o.ProductName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreProductMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreProductMapForViewDto> GetStoreProductMapForView(long id)
        {
            var storeProductMap = await _storeProductMapRepository.GetAsync(id);

            var output = new GetStoreProductMapForViewDto { StoreProductMap = ObjectMapper.Map<StoreProductMapDto>(storeProductMap) };

            if (output.StoreProductMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreProductMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreProductMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.StoreProductMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreProductMaps_Edit)]
        public async Task<GetStoreProductMapForEditOutput> GetStoreProductMapForEdit(EntityDto<long> input)
        {
            var storeProductMap = await _storeProductMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreProductMapForEditOutput { StoreProductMap = ObjectMapper.Map<CreateOrEditStoreProductMapDto>(storeProductMap) };

            if (output.StoreProductMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreProductMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreProductMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.StoreProductMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreProductMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreProductMaps_Create)]
        protected virtual async Task Create(CreateOrEditStoreProductMapDto input)
        {
            var storeProductMap = ObjectMapper.Map<StoreProductMap>(input);

            if (AbpSession.TenantId != null)
            {
                storeProductMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeProductMapRepository.InsertAsync(storeProductMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreProductMaps_Edit)]
        protected virtual async Task Update(CreateOrEditStoreProductMapDto input)
        {
            var storeProductMap = await _storeProductMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeProductMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreProductMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeProductMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreProductMapsToExcel(GetAllStoreProductMapsForExcelInput input)
        {

            var filteredStoreProductMaps = _storeProductMapRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var query = (from o in filteredStoreProductMaps
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStoreProductMapForViewDto()
                         {
                             StoreProductMap = new StoreProductMapDto
                             {
                                 Published = o.Published,
                                 DisplaySequence = o.DisplaySequence,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var storeProductMapListDtos = await query.ToListAsync();

            return _storeProductMapsExcelExporter.ExportToFile(storeProductMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreProductMaps)]
        public async Task<PagedResultDto<StoreProductMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreProductMapStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreProductMapStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreProductMapStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreProductMaps)]
        public async Task<PagedResultDto<StoreProductMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreProductMapProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new StoreProductMapProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreProductMapProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}