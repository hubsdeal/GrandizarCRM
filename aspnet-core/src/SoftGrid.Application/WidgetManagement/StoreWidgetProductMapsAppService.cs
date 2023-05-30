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
    [AbpAuthorize(AppPermissions.Pages_StoreWidgetProductMaps)]
    public class StoreWidgetProductMapsAppService : SoftGridAppServiceBase, IStoreWidgetProductMapsAppService
    {
        private readonly IRepository<StoreWidgetProductMap, long> _storeWidgetProductMapRepository;
        private readonly IStoreWidgetProductMapsExcelExporter _storeWidgetProductMapsExcelExporter;
        private readonly IRepository<StoreWidgetMap, long> _lookup_storeWidgetMapRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;

        public StoreWidgetProductMapsAppService(IRepository<StoreWidgetProductMap, long> storeWidgetProductMapRepository, IStoreWidgetProductMapsExcelExporter storeWidgetProductMapsExcelExporter, IRepository<StoreWidgetMap, long> lookup_storeWidgetMapRepository, IRepository<Product, long> lookup_productRepository)
        {
            _storeWidgetProductMapRepository = storeWidgetProductMapRepository;
            _storeWidgetProductMapsExcelExporter = storeWidgetProductMapsExcelExporter;
            _lookup_storeWidgetMapRepository = lookup_storeWidgetMapRepository;
            _lookup_productRepository = lookup_productRepository;

        }

        public async Task<PagedResultDto<GetStoreWidgetProductMapForViewDto>> GetAll(GetAllStoreWidgetProductMapsInput input)
        {

            var filteredStoreWidgetProductMaps = _storeWidgetProductMapRepository.GetAll()
                        .Include(e => e.StoreWidgetMapFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreWidgetMapCustomNameFilter), e => e.StoreWidgetMapFk != null && e.StoreWidgetMapFk.CustomName == input.StoreWidgetMapCustomNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var pagedAndFilteredStoreWidgetProductMaps = filteredStoreWidgetProductMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeWidgetProductMaps = from o in pagedAndFilteredStoreWidgetProductMaps
                                         join o1 in _lookup_storeWidgetMapRepository.GetAll() on o.StoreWidgetMapId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()

                                         select new
                                         {

                                             o.DisplaySequence,
                                             Id = o.Id,
                                             StoreWidgetMapCustomName = s1 == null || s1.CustomName == null ? "" : s1.CustomName.ToString(),
                                             ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                         };

            var totalCount = await filteredStoreWidgetProductMaps.CountAsync();

            var dbList = await storeWidgetProductMaps.ToListAsync();
            var results = new List<GetStoreWidgetProductMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreWidgetProductMapForViewDto()
                {
                    StoreWidgetProductMap = new StoreWidgetProductMapDto
                    {

                        DisplaySequence = o.DisplaySequence,
                        Id = o.Id,
                    },
                    StoreWidgetMapCustomName = o.StoreWidgetMapCustomName,
                    ProductName = o.ProductName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreWidgetProductMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreWidgetProductMapForViewDto> GetStoreWidgetProductMapForView(long id)
        {
            var storeWidgetProductMap = await _storeWidgetProductMapRepository.GetAsync(id);

            var output = new GetStoreWidgetProductMapForViewDto { StoreWidgetProductMap = ObjectMapper.Map<StoreWidgetProductMapDto>(storeWidgetProductMap) };

            if (output.StoreWidgetProductMap.StoreWidgetMapId != null)
            {
                var _lookupStoreWidgetMap = await _lookup_storeWidgetMapRepository.FirstOrDefaultAsync((long)output.StoreWidgetProductMap.StoreWidgetMapId);
                output.StoreWidgetMapCustomName = _lookupStoreWidgetMap?.CustomName?.ToString();
            }

            if (output.StoreWidgetProductMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.StoreWidgetProductMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetProductMaps_Edit)]
        public async Task<GetStoreWidgetProductMapForEditOutput> GetStoreWidgetProductMapForEdit(EntityDto<long> input)
        {
            var storeWidgetProductMap = await _storeWidgetProductMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreWidgetProductMapForEditOutput { StoreWidgetProductMap = ObjectMapper.Map<CreateOrEditStoreWidgetProductMapDto>(storeWidgetProductMap) };

            if (output.StoreWidgetProductMap.StoreWidgetMapId != null)
            {
                var _lookupStoreWidgetMap = await _lookup_storeWidgetMapRepository.FirstOrDefaultAsync((long)output.StoreWidgetProductMap.StoreWidgetMapId);
                output.StoreWidgetMapCustomName = _lookupStoreWidgetMap?.CustomName?.ToString();
            }

            if (output.StoreWidgetProductMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.StoreWidgetProductMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreWidgetProductMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetProductMaps_Create)]
        protected virtual async Task Create(CreateOrEditStoreWidgetProductMapDto input)
        {
            var storeWidgetProductMap = ObjectMapper.Map<StoreWidgetProductMap>(input);

            if (AbpSession.TenantId != null)
            {
                storeWidgetProductMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeWidgetProductMapRepository.InsertAsync(storeWidgetProductMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetProductMaps_Edit)]
        protected virtual async Task Update(CreateOrEditStoreWidgetProductMapDto input)
        {
            var storeWidgetProductMap = await _storeWidgetProductMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeWidgetProductMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetProductMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeWidgetProductMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreWidgetProductMapsToExcel(GetAllStoreWidgetProductMapsForExcelInput input)
        {

            var filteredStoreWidgetProductMaps = _storeWidgetProductMapRepository.GetAll()
                        .Include(e => e.StoreWidgetMapFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreWidgetMapCustomNameFilter), e => e.StoreWidgetMapFk != null && e.StoreWidgetMapFk.CustomName == input.StoreWidgetMapCustomNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var query = (from o in filteredStoreWidgetProductMaps
                         join o1 in _lookup_storeWidgetMapRepository.GetAll() on o.StoreWidgetMapId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStoreWidgetProductMapForViewDto()
                         {
                             StoreWidgetProductMap = new StoreWidgetProductMapDto
                             {
                                 DisplaySequence = o.DisplaySequence,
                                 Id = o.Id
                             },
                             StoreWidgetMapCustomName = s1 == null || s1.CustomName == null ? "" : s1.CustomName.ToString(),
                             ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var storeWidgetProductMapListDtos = await query.ToListAsync();

            return _storeWidgetProductMapsExcelExporter.ExportToFile(storeWidgetProductMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetProductMaps)]
        public async Task<PagedResultDto<StoreWidgetProductMapStoreWidgetMapLookupTableDto>> GetAllStoreWidgetMapForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeWidgetMapRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.CustomName != null && e.CustomName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeWidgetMapList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreWidgetProductMapStoreWidgetMapLookupTableDto>();
            foreach (var storeWidgetMap in storeWidgetMapList)
            {
                lookupTableDtoList.Add(new StoreWidgetProductMapStoreWidgetMapLookupTableDto
                {
                    Id = storeWidgetMap.Id,
                    DisplayName = storeWidgetMap.CustomName?.ToString()
                });
            }

            return new PagedResultDto<StoreWidgetProductMapStoreWidgetMapLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetProductMaps)]
        public async Task<PagedResultDto<StoreWidgetProductMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreWidgetProductMapProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new StoreWidgetProductMapProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreWidgetProductMapProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}