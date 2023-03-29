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
    [AbpAuthorize(AppPermissions.Pages_ProductSubscriptionMaps)]
    public class ProductSubscriptionMapsAppService : SoftGridAppServiceBase, IProductSubscriptionMapsAppService
    {
        private readonly IRepository<ProductSubscriptionMap, long> _productSubscriptionMapRepository;
        private readonly IProductSubscriptionMapsExcelExporter _productSubscriptionMapsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<SubscriptionType, long> _lookup_subscriptionTypeRepository;

        public ProductSubscriptionMapsAppService(IRepository<ProductSubscriptionMap, long> productSubscriptionMapRepository, IProductSubscriptionMapsExcelExporter productSubscriptionMapsExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<SubscriptionType, long> lookup_subscriptionTypeRepository)
        {
            _productSubscriptionMapRepository = productSubscriptionMapRepository;
            _productSubscriptionMapsExcelExporter = productSubscriptionMapsExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_subscriptionTypeRepository = lookup_subscriptionTypeRepository;

        }

        public async Task<PagedResultDto<GetProductSubscriptionMapForViewDto>> GetAll(GetAllProductSubscriptionMapsInput input)
        {

            var filteredProductSubscriptionMaps = _productSubscriptionMapRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.SubscriptionTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDiscountPercentageFilter != null, e => e.DiscountPercentage >= input.MinDiscountPercentageFilter)
                        .WhereIf(input.MaxDiscountPercentageFilter != null, e => e.DiscountPercentage <= input.MaxDiscountPercentageFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubscriptionTypeNameFilter), e => e.SubscriptionTypeFk != null && e.SubscriptionTypeFk.Name == input.SubscriptionTypeNameFilter);

            var pagedAndFilteredProductSubscriptionMaps = filteredProductSubscriptionMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productSubscriptionMaps = from o in pagedAndFilteredProductSubscriptionMaps
                                          join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                          from s1 in j1.DefaultIfEmpty()

                                          join o2 in _lookup_subscriptionTypeRepository.GetAll() on o.SubscriptionTypeId equals o2.Id into j2
                                          from s2 in j2.DefaultIfEmpty()

                                          select new
                                          {

                                              o.DiscountPercentage,
                                              o.DiscountAmount,
                                              o.Price,
                                              Id = o.Id,
                                              ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                              SubscriptionTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                          };

            var totalCount = await filteredProductSubscriptionMaps.CountAsync();

            var dbList = await productSubscriptionMaps.ToListAsync();
            var results = new List<GetProductSubscriptionMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductSubscriptionMapForViewDto()
                {
                    ProductSubscriptionMap = new ProductSubscriptionMapDto
                    {

                        DiscountPercentage = o.DiscountPercentage,
                        DiscountAmount = o.DiscountAmount,
                        Price = o.Price,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    SubscriptionTypeName = o.SubscriptionTypeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductSubscriptionMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductSubscriptionMapForViewDto> GetProductSubscriptionMapForView(long id)
        {
            var productSubscriptionMap = await _productSubscriptionMapRepository.GetAsync(id);

            var output = new GetProductSubscriptionMapForViewDto { ProductSubscriptionMap = ObjectMapper.Map<ProductSubscriptionMapDto>(productSubscriptionMap) };

            if (output.ProductSubscriptionMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductSubscriptionMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductSubscriptionMap.SubscriptionTypeId != null)
            {
                var _lookupSubscriptionType = await _lookup_subscriptionTypeRepository.FirstOrDefaultAsync((long)output.ProductSubscriptionMap.SubscriptionTypeId);
                output.SubscriptionTypeName = _lookupSubscriptionType?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductSubscriptionMaps_Edit)]
        public async Task<GetProductSubscriptionMapForEditOutput> GetProductSubscriptionMapForEdit(EntityDto<long> input)
        {
            var productSubscriptionMap = await _productSubscriptionMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductSubscriptionMapForEditOutput { ProductSubscriptionMap = ObjectMapper.Map<CreateOrEditProductSubscriptionMapDto>(productSubscriptionMap) };

            if (output.ProductSubscriptionMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductSubscriptionMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductSubscriptionMap.SubscriptionTypeId != null)
            {
                var _lookupSubscriptionType = await _lookup_subscriptionTypeRepository.FirstOrDefaultAsync((long)output.ProductSubscriptionMap.SubscriptionTypeId);
                output.SubscriptionTypeName = _lookupSubscriptionType?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductSubscriptionMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductSubscriptionMaps_Create)]
        protected virtual async Task Create(CreateOrEditProductSubscriptionMapDto input)
        {
            var productSubscriptionMap = ObjectMapper.Map<ProductSubscriptionMap>(input);

            if (AbpSession.TenantId != null)
            {
                productSubscriptionMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _productSubscriptionMapRepository.InsertAsync(productSubscriptionMap);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductSubscriptionMaps_Edit)]
        protected virtual async Task Update(CreateOrEditProductSubscriptionMapDto input)
        {
            var productSubscriptionMap = await _productSubscriptionMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productSubscriptionMap);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductSubscriptionMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productSubscriptionMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductSubscriptionMapsToExcel(GetAllProductSubscriptionMapsForExcelInput input)
        {

            var filteredProductSubscriptionMaps = _productSubscriptionMapRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.SubscriptionTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDiscountPercentageFilter != null, e => e.DiscountPercentage >= input.MinDiscountPercentageFilter)
                        .WhereIf(input.MaxDiscountPercentageFilter != null, e => e.DiscountPercentage <= input.MaxDiscountPercentageFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubscriptionTypeNameFilter), e => e.SubscriptionTypeFk != null && e.SubscriptionTypeFk.Name == input.SubscriptionTypeNameFilter);

            var query = (from o in filteredProductSubscriptionMaps
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_subscriptionTypeRepository.GetAll() on o.SubscriptionTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProductSubscriptionMapForViewDto()
                         {
                             ProductSubscriptionMap = new ProductSubscriptionMapDto
                             {
                                 DiscountPercentage = o.DiscountPercentage,
                                 DiscountAmount = o.DiscountAmount,
                                 Price = o.Price,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             SubscriptionTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var productSubscriptionMapListDtos = await query.ToListAsync();

            return _productSubscriptionMapsExcelExporter.ExportToFile(productSubscriptionMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductSubscriptionMaps)]
        public async Task<PagedResultDto<ProductSubscriptionMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductSubscriptionMapProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductSubscriptionMapProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductSubscriptionMapProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductSubscriptionMaps)]
        public async Task<PagedResultDto<ProductSubscriptionMapSubscriptionTypeLookupTableDto>> GetAllSubscriptionTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_subscriptionTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var subscriptionTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductSubscriptionMapSubscriptionTypeLookupTableDto>();
            foreach (var subscriptionType in subscriptionTypeList)
            {
                lookupTableDtoList.Add(new ProductSubscriptionMapSubscriptionTypeLookupTableDto
                {
                    Id = subscriptionType.Id,
                    DisplayName = subscriptionType.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductSubscriptionMapSubscriptionTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}