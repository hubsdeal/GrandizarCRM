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
    [AbpAuthorize(AppPermissions.Pages_StoreProductCategoryMaps)]
    public class StoreProductCategoryMapsAppService : SoftGridAppServiceBase, IStoreProductCategoryMapsAppService
    {
        private readonly IRepository<StoreProductCategoryMap, long> _storeProductCategoryMapRepository;
        private readonly IStoreProductCategoryMapsExcelExporter _storeProductCategoryMapsExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<ProductCategory, long> _lookup_productCategoryRepository;

        public StoreProductCategoryMapsAppService(IRepository<StoreProductCategoryMap, long> storeProductCategoryMapRepository, IStoreProductCategoryMapsExcelExporter storeProductCategoryMapsExcelExporter, IRepository<Store, long> lookup_storeRepository, IRepository<ProductCategory, long> lookup_productCategoryRepository)
        {
            _storeProductCategoryMapRepository = storeProductCategoryMapRepository;
            _storeProductCategoryMapsExcelExporter = storeProductCategoryMapsExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;

        }

        public async Task<PagedResultDto<GetStoreProductCategoryMapForViewDto>> GetAll(GetAllStoreProductCategoryMapsInput input)
        {

            var filteredStoreProductCategoryMaps = _storeProductCategoryMapRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter);

            var pagedAndFilteredStoreProductCategoryMaps = filteredStoreProductCategoryMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeProductCategoryMaps = from o in pagedAndFilteredStoreProductCategoryMaps
                                           join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                                           from s1 in j1.DefaultIfEmpty()

                                           join o2 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o2.Id into j2
                                           from s2 in j2.DefaultIfEmpty()

                                           select new
                                           {

                                               o.Published,
                                               o.DisplaySequence,
                                               Id = o.Id,
                                               StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                               ProductCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                           };

            var totalCount = await filteredStoreProductCategoryMaps.CountAsync();

            var dbList = await storeProductCategoryMaps.ToListAsync();
            var results = new List<GetStoreProductCategoryMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreProductCategoryMapForViewDto()
                {
                    StoreProductCategoryMap = new StoreProductCategoryMapDto
                    {

                        Published = o.Published,
                        DisplaySequence = o.DisplaySequence,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName,
                    ProductCategoryName = o.ProductCategoryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreProductCategoryMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreProductCategoryMapForViewDto> GetStoreProductCategoryMapForView(long id)
        {
            var storeProductCategoryMap = await _storeProductCategoryMapRepository.GetAsync(id);

            var output = new GetStoreProductCategoryMapForViewDto { StoreProductCategoryMap = ObjectMapper.Map<StoreProductCategoryMapDto>(storeProductCategoryMap) };

            if (output.StoreProductCategoryMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreProductCategoryMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreProductCategoryMap.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.StoreProductCategoryMap.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreProductCategoryMaps_Edit)]
        public async Task<GetStoreProductCategoryMapForEditOutput> GetStoreProductCategoryMapForEdit(EntityDto<long> input)
        {
            var storeProductCategoryMap = await _storeProductCategoryMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreProductCategoryMapForEditOutput { StoreProductCategoryMap = ObjectMapper.Map<CreateOrEditStoreProductCategoryMapDto>(storeProductCategoryMap) };

            if (output.StoreProductCategoryMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreProductCategoryMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreProductCategoryMap.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.StoreProductCategoryMap.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreProductCategoryMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreProductCategoryMaps_Create)]
        protected virtual async Task Create(CreateOrEditStoreProductCategoryMapDto input)
        {
            var storeProductCategoryMap = ObjectMapper.Map<StoreProductCategoryMap>(input);

            if (AbpSession.TenantId != null)
            {
                storeProductCategoryMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeProductCategoryMapRepository.InsertAsync(storeProductCategoryMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreProductCategoryMaps_Edit)]
        protected virtual async Task Update(CreateOrEditStoreProductCategoryMapDto input)
        {
            var storeProductCategoryMap = await _storeProductCategoryMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeProductCategoryMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreProductCategoryMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeProductCategoryMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreProductCategoryMapsToExcel(GetAllStoreProductCategoryMapsForExcelInput input)
        {

            var filteredStoreProductCategoryMaps = _storeProductCategoryMapRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter);

            var query = (from o in filteredStoreProductCategoryMaps
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStoreProductCategoryMapForViewDto()
                         {
                             StoreProductCategoryMap = new StoreProductCategoryMapDto
                             {
                                 Published = o.Published,
                                 DisplaySequence = o.DisplaySequence,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var storeProductCategoryMapListDtos = await query.ToListAsync();

            return _storeProductCategoryMapsExcelExporter.ExportToFile(storeProductCategoryMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreProductCategoryMaps)]
        public async Task<PagedResultDto<StoreProductCategoryMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreProductCategoryMapStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreProductCategoryMapStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreProductCategoryMapStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreProductCategoryMaps)]
        public async Task<PagedResultDto<StoreProductCategoryMapProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreProductCategoryMapProductCategoryLookupTableDto>();
            foreach (var productCategory in productCategoryList)
            {
                lookupTableDtoList.Add(new StoreProductCategoryMapProductCategoryLookupTableDto
                {
                    Id = productCategory.Id,
                    DisplayName = productCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreProductCategoryMapProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}