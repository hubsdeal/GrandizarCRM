using SoftGrid.Shop;
using SoftGrid.Shop;
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
    [AbpAuthorize(AppPermissions.Pages_StoreMarketplaceCommissionSettings)]
    public class StoreMarketplaceCommissionSettingsAppService : SoftGridAppServiceBase, IStoreMarketplaceCommissionSettingsAppService
    {
        private readonly IRepository<StoreMarketplaceCommissionSetting, long> _storeMarketplaceCommissionSettingRepository;
        private readonly IStoreMarketplaceCommissionSettingsExcelExporter _storeMarketplaceCommissionSettingsExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<MarketplaceCommissionType, long> _lookup_marketplaceCommissionTypeRepository;
        private readonly IRepository<ProductCategory, long> _lookup_productCategoryRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;

        public StoreMarketplaceCommissionSettingsAppService(IRepository<StoreMarketplaceCommissionSetting, long> storeMarketplaceCommissionSettingRepository, IStoreMarketplaceCommissionSettingsExcelExporter storeMarketplaceCommissionSettingsExcelExporter, IRepository<Store, long> lookup_storeRepository, IRepository<MarketplaceCommissionType, long> lookup_marketplaceCommissionTypeRepository, IRepository<ProductCategory, long> lookup_productCategoryRepository, IRepository<Product, long> lookup_productRepository)
        {
            _storeMarketplaceCommissionSettingRepository = storeMarketplaceCommissionSettingRepository;
            _storeMarketplaceCommissionSettingsExcelExporter = storeMarketplaceCommissionSettingsExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_marketplaceCommissionTypeRepository = lookup_marketplaceCommissionTypeRepository;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;
            _lookup_productRepository = lookup_productRepository;

        }

        public async Task<PagedResultDto<GetStoreMarketplaceCommissionSettingForViewDto>> GetAll(GetAllStoreMarketplaceCommissionSettingsInput input)
        {

            var filteredStoreMarketplaceCommissionSettings = _storeMarketplaceCommissionSettingRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.MarketplaceCommissionTypeFk)
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinPercentageFilter != null, e => e.Percentage >= input.MinPercentageFilter)
                        .WhereIf(input.MaxPercentageFilter != null, e => e.Percentage <= input.MaxPercentageFilter)
                        .WhereIf(input.MinFixedAmountFilter != null, e => e.FixedAmount >= input.MinFixedAmountFilter)
                        .WhereIf(input.MaxFixedAmountFilter != null, e => e.FixedAmount <= input.MaxFixedAmountFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MarketplaceCommissionTypeNameFilter), e => e.MarketplaceCommissionTypeFk != null && e.MarketplaceCommissionTypeFk.Name == input.MarketplaceCommissionTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var pagedAndFilteredStoreMarketplaceCommissionSettings = filteredStoreMarketplaceCommissionSettings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeMarketplaceCommissionSettings = from o in pagedAndFilteredStoreMarketplaceCommissionSettings
                                                     join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                                                     from s1 in j1.DefaultIfEmpty()

                                                     join o2 in _lookup_marketplaceCommissionTypeRepository.GetAll() on o.MarketplaceCommissionTypeId equals o2.Id into j2
                                                     from s2 in j2.DefaultIfEmpty()

                                                     join o3 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o3.Id into j3
                                                     from s3 in j3.DefaultIfEmpty()

                                                     join o4 in _lookup_productRepository.GetAll() on o.ProductId equals o4.Id into j4
                                                     from s4 in j4.DefaultIfEmpty()

                                                     select new
                                                     {

                                                         o.Percentage,
                                                         o.FixedAmount,
                                                         o.StartDate,
                                                         o.EndDate,
                                                         Id = o.Id,
                                                         StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                                         MarketplaceCommissionTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                                         ProductCategoryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                                         ProductName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                                     };

            var totalCount = await filteredStoreMarketplaceCommissionSettings.CountAsync();

            var dbList = await storeMarketplaceCommissionSettings.ToListAsync();
            var results = new List<GetStoreMarketplaceCommissionSettingForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreMarketplaceCommissionSettingForViewDto()
                {
                    StoreMarketplaceCommissionSetting = new StoreMarketplaceCommissionSettingDto
                    {

                        Percentage = o.Percentage,
                        FixedAmount = o.FixedAmount,
                        StartDate = o.StartDate,
                        EndDate = o.EndDate,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName,
                    MarketplaceCommissionTypeName = o.MarketplaceCommissionTypeName,
                    ProductCategoryName = o.ProductCategoryName,
                    ProductName = o.ProductName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreMarketplaceCommissionSettingForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreMarketplaceCommissionSettingForViewDto> GetStoreMarketplaceCommissionSettingForView(long id)
        {
            var storeMarketplaceCommissionSetting = await _storeMarketplaceCommissionSettingRepository.GetAsync(id);

            var output = new GetStoreMarketplaceCommissionSettingForViewDto { StoreMarketplaceCommissionSetting = ObjectMapper.Map<StoreMarketplaceCommissionSettingDto>(storeMarketplaceCommissionSetting) };

            if (output.StoreMarketplaceCommissionSetting.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreMarketplaceCommissionSetting.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreMarketplaceCommissionSetting.MarketplaceCommissionTypeId != null)
            {
                var _lookupMarketplaceCommissionType = await _lookup_marketplaceCommissionTypeRepository.FirstOrDefaultAsync((long)output.StoreMarketplaceCommissionSetting.MarketplaceCommissionTypeId);
                output.MarketplaceCommissionTypeName = _lookupMarketplaceCommissionType?.Name?.ToString();
            }

            if (output.StoreMarketplaceCommissionSetting.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.StoreMarketplaceCommissionSetting.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.StoreMarketplaceCommissionSetting.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.StoreMarketplaceCommissionSetting.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreMarketplaceCommissionSettings_Edit)]
        public async Task<GetStoreMarketplaceCommissionSettingForEditOutput> GetStoreMarketplaceCommissionSettingForEdit(EntityDto<long> input)
        {
            var storeMarketplaceCommissionSetting = await _storeMarketplaceCommissionSettingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreMarketplaceCommissionSettingForEditOutput { StoreMarketplaceCommissionSetting = ObjectMapper.Map<CreateOrEditStoreMarketplaceCommissionSettingDto>(storeMarketplaceCommissionSetting) };

            if (output.StoreMarketplaceCommissionSetting.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreMarketplaceCommissionSetting.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreMarketplaceCommissionSetting.MarketplaceCommissionTypeId != null)
            {
                var _lookupMarketplaceCommissionType = await _lookup_marketplaceCommissionTypeRepository.FirstOrDefaultAsync((long)output.StoreMarketplaceCommissionSetting.MarketplaceCommissionTypeId);
                output.MarketplaceCommissionTypeName = _lookupMarketplaceCommissionType?.Name?.ToString();
            }

            if (output.StoreMarketplaceCommissionSetting.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.StoreMarketplaceCommissionSetting.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.StoreMarketplaceCommissionSetting.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.StoreMarketplaceCommissionSetting.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreMarketplaceCommissionSettingDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreMarketplaceCommissionSettings_Create)]
        protected virtual async Task Create(CreateOrEditStoreMarketplaceCommissionSettingDto input)
        {
            var storeMarketplaceCommissionSetting = ObjectMapper.Map<StoreMarketplaceCommissionSetting>(input);

            if (AbpSession.TenantId != null)
            {
                storeMarketplaceCommissionSetting.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeMarketplaceCommissionSettingRepository.InsertAsync(storeMarketplaceCommissionSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreMarketplaceCommissionSettings_Edit)]
        protected virtual async Task Update(CreateOrEditStoreMarketplaceCommissionSettingDto input)
        {
            var storeMarketplaceCommissionSetting = await _storeMarketplaceCommissionSettingRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeMarketplaceCommissionSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreMarketplaceCommissionSettings_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeMarketplaceCommissionSettingRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreMarketplaceCommissionSettingsToExcel(GetAllStoreMarketplaceCommissionSettingsForExcelInput input)
        {

            var filteredStoreMarketplaceCommissionSettings = _storeMarketplaceCommissionSettingRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.MarketplaceCommissionTypeFk)
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinPercentageFilter != null, e => e.Percentage >= input.MinPercentageFilter)
                        .WhereIf(input.MaxPercentageFilter != null, e => e.Percentage <= input.MaxPercentageFilter)
                        .WhereIf(input.MinFixedAmountFilter != null, e => e.FixedAmount >= input.MinFixedAmountFilter)
                        .WhereIf(input.MaxFixedAmountFilter != null, e => e.FixedAmount <= input.MaxFixedAmountFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MarketplaceCommissionTypeNameFilter), e => e.MarketplaceCommissionTypeFk != null && e.MarketplaceCommissionTypeFk.Name == input.MarketplaceCommissionTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var query = (from o in filteredStoreMarketplaceCommissionSettings
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_marketplaceCommissionTypeRepository.GetAll() on o.MarketplaceCommissionTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_productRepository.GetAll() on o.ProductId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetStoreMarketplaceCommissionSettingForViewDto()
                         {
                             StoreMarketplaceCommissionSetting = new StoreMarketplaceCommissionSettingDto
                             {
                                 Percentage = o.Percentage,
                                 FixedAmount = o.FixedAmount,
                                 StartDate = o.StartDate,
                                 EndDate = o.EndDate,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MarketplaceCommissionTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ProductCategoryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             ProductName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var storeMarketplaceCommissionSettingListDtos = await query.ToListAsync();

            return _storeMarketplaceCommissionSettingsExcelExporter.ExportToFile(storeMarketplaceCommissionSettingListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreMarketplaceCommissionSettings)]
        public async Task<PagedResultDto<StoreMarketplaceCommissionSettingStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreMarketplaceCommissionSettingStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreMarketplaceCommissionSettingStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreMarketplaceCommissionSettingStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreMarketplaceCommissionSettings)]
        public async Task<PagedResultDto<StoreMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableDto>> GetAllMarketplaceCommissionTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_marketplaceCommissionTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var marketplaceCommissionTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableDto>();
            foreach (var marketplaceCommissionType in marketplaceCommissionTypeList)
            {
                lookupTableDtoList.Add(new StoreMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableDto
                {
                    Id = marketplaceCommissionType.Id,
                    DisplayName = marketplaceCommissionType.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreMarketplaceCommissionSettings)]
        public async Task<PagedResultDto<StoreMarketplaceCommissionSettingProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreMarketplaceCommissionSettingProductCategoryLookupTableDto>();
            foreach (var productCategory in productCategoryList)
            {
                lookupTableDtoList.Add(new StoreMarketplaceCommissionSettingProductCategoryLookupTableDto
                {
                    Id = productCategory.Id,
                    DisplayName = productCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreMarketplaceCommissionSettingProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreMarketplaceCommissionSettings)]
        public async Task<PagedResultDto<StoreMarketplaceCommissionSettingProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreMarketplaceCommissionSettingProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new StoreMarketplaceCommissionSettingProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreMarketplaceCommissionSettingProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}