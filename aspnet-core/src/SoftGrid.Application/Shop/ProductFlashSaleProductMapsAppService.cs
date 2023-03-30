using SoftGrid.Shop;
using SoftGrid.Shop;
using SoftGrid.LookupData;

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
    [AbpAuthorize(AppPermissions.Pages_ProductFlashSaleProductMaps)]
    public class ProductFlashSaleProductMapsAppService : SoftGridAppServiceBase, IProductFlashSaleProductMapsAppService
    {
        private readonly IRepository<ProductFlashSaleProductMap, long> _productFlashSaleProductMapRepository;
        private readonly IProductFlashSaleProductMapsExcelExporter _productFlashSaleProductMapsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<MembershipType, long> _lookup_membershipTypeRepository;

        public ProductFlashSaleProductMapsAppService(IRepository<ProductFlashSaleProductMap, long> productFlashSaleProductMapRepository, IProductFlashSaleProductMapsExcelExporter productFlashSaleProductMapsExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<Store, long> lookup_storeRepository, IRepository<MembershipType, long> lookup_membershipTypeRepository)
        {
            _productFlashSaleProductMapRepository = productFlashSaleProductMapRepository;
            _productFlashSaleProductMapsExcelExporter = productFlashSaleProductMapsExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_membershipTypeRepository = lookup_membershipTypeRepository;

        }

        public async Task<PagedResultDto<GetProductFlashSaleProductMapForViewDto>> GetAll(GetAllProductFlashSaleProductMapsInput input)
        {

            var filteredProductFlashSaleProductMaps = _productFlashSaleProductMapRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.MembershipTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.EndTime.Contains(input.Filter) || e.StartTime.Contains(input.Filter))
                        .WhereIf(input.MinFlashSalePriceFilter != null, e => e.FlashSalePrice >= input.MinFlashSalePriceFilter)
                        .WhereIf(input.MaxFlashSalePriceFilter != null, e => e.FlashSalePrice <= input.MaxFlashSalePriceFilter)
                        .WhereIf(input.MinDiscountPercentageFilter != null, e => e.DiscountPercentage >= input.MinDiscountPercentageFilter)
                        .WhereIf(input.MaxDiscountPercentageFilter != null, e => e.DiscountPercentage <= input.MaxDiscountPercentageFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EndTimeFilter), e => e.EndTime.Contains(input.EndTimeFilter))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StartTimeFilter), e => e.StartTime.Contains(input.StartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MembershipTypeNameFilter), e => e.MembershipTypeFk != null && e.MembershipTypeFk.Name == input.MembershipTypeNameFilter);

            var pagedAndFilteredProductFlashSaleProductMaps = filteredProductFlashSaleProductMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productFlashSaleProductMaps = from o in pagedAndFilteredProductFlashSaleProductMaps
                                              join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                              from s1 in j1.DefaultIfEmpty()

                                              join o2 in _lookup_storeRepository.GetAll() on o.StoreId equals o2.Id into j2
                                              from s2 in j2.DefaultIfEmpty()

                                              join o3 in _lookup_membershipTypeRepository.GetAll() on o.MembershipTypeId equals o3.Id into j3
                                              from s3 in j3.DefaultIfEmpty()

                                              select new
                                              {

                                                  o.FlashSalePrice,
                                                  o.DiscountPercentage,
                                                  o.DiscountAmount,
                                                  o.EndDate,
                                                  o.EndTime,
                                                  o.StartDate,
                                                  o.StartTime,
                                                  Id = o.Id,
                                                  ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                                  StoreName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                                  MembershipTypeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                              };

            var totalCount = await filteredProductFlashSaleProductMaps.CountAsync();

            var dbList = await productFlashSaleProductMaps.ToListAsync();
            var results = new List<GetProductFlashSaleProductMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductFlashSaleProductMapForViewDto()
                {
                    ProductFlashSaleProductMap = new ProductFlashSaleProductMapDto
                    {

                        FlashSalePrice = o.FlashSalePrice,
                        DiscountPercentage = o.DiscountPercentage,
                        DiscountAmount = o.DiscountAmount,
                        EndDate = o.EndDate,
                        EndTime = o.EndTime,
                        StartDate = o.StartDate,
                        StartTime = o.StartTime,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    StoreName = o.StoreName,
                    MembershipTypeName = o.MembershipTypeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductFlashSaleProductMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductFlashSaleProductMapForViewDto> GetProductFlashSaleProductMapForView(long id)
        {
            var productFlashSaleProductMap = await _productFlashSaleProductMapRepository.GetAsync(id);

            var output = new GetProductFlashSaleProductMapForViewDto { ProductFlashSaleProductMap = ObjectMapper.Map<ProductFlashSaleProductMapDto>(productFlashSaleProductMap) };

            if (output.ProductFlashSaleProductMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductFlashSaleProductMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductFlashSaleProductMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.ProductFlashSaleProductMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.ProductFlashSaleProductMap.MembershipTypeId != null)
            {
                var _lookupMembershipType = await _lookup_membershipTypeRepository.FirstOrDefaultAsync((long)output.ProductFlashSaleProductMap.MembershipTypeId);
                output.MembershipTypeName = _lookupMembershipType?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductFlashSaleProductMaps_Edit)]
        public async Task<GetProductFlashSaleProductMapForEditOutput> GetProductFlashSaleProductMapForEdit(EntityDto<long> input)
        {
            var productFlashSaleProductMap = await _productFlashSaleProductMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductFlashSaleProductMapForEditOutput { ProductFlashSaleProductMap = ObjectMapper.Map<CreateOrEditProductFlashSaleProductMapDto>(productFlashSaleProductMap) };

            if (output.ProductFlashSaleProductMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductFlashSaleProductMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductFlashSaleProductMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.ProductFlashSaleProductMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.ProductFlashSaleProductMap.MembershipTypeId != null)
            {
                var _lookupMembershipType = await _lookup_membershipTypeRepository.FirstOrDefaultAsync((long)output.ProductFlashSaleProductMap.MembershipTypeId);
                output.MembershipTypeName = _lookupMembershipType?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductFlashSaleProductMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductFlashSaleProductMaps_Create)]
        protected virtual async Task Create(CreateOrEditProductFlashSaleProductMapDto input)
        {
            var productFlashSaleProductMap = ObjectMapper.Map<ProductFlashSaleProductMap>(input);

            if (AbpSession.TenantId != null)
            {
                productFlashSaleProductMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _productFlashSaleProductMapRepository.InsertAsync(productFlashSaleProductMap);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductFlashSaleProductMaps_Edit)]
        protected virtual async Task Update(CreateOrEditProductFlashSaleProductMapDto input)
        {
            var productFlashSaleProductMap = await _productFlashSaleProductMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productFlashSaleProductMap);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductFlashSaleProductMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productFlashSaleProductMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductFlashSaleProductMapsToExcel(GetAllProductFlashSaleProductMapsForExcelInput input)
        {

            var filteredProductFlashSaleProductMaps = _productFlashSaleProductMapRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.MembershipTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.EndTime.Contains(input.Filter) || e.StartTime.Contains(input.Filter))
                        .WhereIf(input.MinFlashSalePriceFilter != null, e => e.FlashSalePrice >= input.MinFlashSalePriceFilter)
                        .WhereIf(input.MaxFlashSalePriceFilter != null, e => e.FlashSalePrice <= input.MaxFlashSalePriceFilter)
                        .WhereIf(input.MinDiscountPercentageFilter != null, e => e.DiscountPercentage >= input.MinDiscountPercentageFilter)
                        .WhereIf(input.MaxDiscountPercentageFilter != null, e => e.DiscountPercentage <= input.MaxDiscountPercentageFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EndTimeFilter), e => e.EndTime.Contains(input.EndTimeFilter))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StartTimeFilter), e => e.StartTime.Contains(input.StartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MembershipTypeNameFilter), e => e.MembershipTypeFk != null && e.MembershipTypeFk.Name == input.MembershipTypeNameFilter);

            var query = (from o in filteredProductFlashSaleProductMaps
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_storeRepository.GetAll() on o.StoreId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_membershipTypeRepository.GetAll() on o.MembershipTypeId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetProductFlashSaleProductMapForViewDto()
                         {
                             ProductFlashSaleProductMap = new ProductFlashSaleProductMapDto
                             {
                                 FlashSalePrice = o.FlashSalePrice,
                                 DiscountPercentage = o.DiscountPercentage,
                                 DiscountAmount = o.DiscountAmount,
                                 EndDate = o.EndDate,
                                 EndTime = o.EndTime,
                                 StartDate = o.StartDate,
                                 StartTime = o.StartTime,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             StoreName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             MembershipTypeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var productFlashSaleProductMapListDtos = await query.ToListAsync();

            return _productFlashSaleProductMapsExcelExporter.ExportToFile(productFlashSaleProductMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductFlashSaleProductMaps)]
        public async Task<PagedResultDto<ProductFlashSaleProductMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductFlashSaleProductMapProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductFlashSaleProductMapProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductFlashSaleProductMapProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductFlashSaleProductMaps)]
        public async Task<PagedResultDto<ProductFlashSaleProductMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductFlashSaleProductMapStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new ProductFlashSaleProductMapStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductFlashSaleProductMapStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductFlashSaleProductMaps)]
        public async Task<PagedResultDto<ProductFlashSaleProductMapMembershipTypeLookupTableDto>> GetAllMembershipTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_membershipTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var membershipTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductFlashSaleProductMapMembershipTypeLookupTableDto>();
            foreach (var membershipType in membershipTypeList)
            {
                lookupTableDtoList.Add(new ProductFlashSaleProductMapMembershipTypeLookupTableDto
                {
                    Id = membershipType.Id,
                    DisplayName = membershipType.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductFlashSaleProductMapMembershipTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}