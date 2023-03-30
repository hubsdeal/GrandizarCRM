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
    [AbpAuthorize(AppPermissions.Pages_ProductAndGiftCardMaps)]
    public class ProductAndGiftCardMapsAppService : SoftGridAppServiceBase, IProductAndGiftCardMapsAppService
    {
        private readonly IRepository<ProductAndGiftCardMap, long> _productAndGiftCardMapRepository;
        private readonly IProductAndGiftCardMapsExcelExporter _productAndGiftCardMapsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<Currency, long> _lookup_currencyRepository;

        public ProductAndGiftCardMapsAppService(IRepository<ProductAndGiftCardMap, long> productAndGiftCardMapRepository, IProductAndGiftCardMapsExcelExporter productAndGiftCardMapsExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<Currency, long> lookup_currencyRepository)
        {
            _productAndGiftCardMapRepository = productAndGiftCardMapRepository;
            _productAndGiftCardMapsExcelExporter = productAndGiftCardMapsExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_currencyRepository = lookup_currencyRepository;

        }

        public async Task<PagedResultDto<GetProductAndGiftCardMapForViewDto>> GetAll(GetAllProductAndGiftCardMapsInput input)
        {

            var filteredProductAndGiftCardMaps = _productAndGiftCardMapRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.CurrencyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinPurchaseAmountFilter != null, e => e.PurchaseAmount >= input.MinPurchaseAmountFilter)
                        .WhereIf(input.MaxPurchaseAmountFilter != null, e => e.PurchaseAmount <= input.MaxPurchaseAmountFilter)
                        .WhereIf(input.MinGiftAmountFilter != null, e => e.GiftAmount >= input.MinGiftAmountFilter)
                        .WhereIf(input.MaxGiftAmountFilter != null, e => e.GiftAmount <= input.MaxGiftAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter);

            var pagedAndFilteredProductAndGiftCardMaps = filteredProductAndGiftCardMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productAndGiftCardMaps = from o in pagedAndFilteredProductAndGiftCardMaps
                                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         join o2 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()

                                         select new
                                         {

                                             o.PurchaseAmount,
                                             o.GiftAmount,
                                             Id = o.Id,
                                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                             CurrencyName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                         };

            var totalCount = await filteredProductAndGiftCardMaps.CountAsync();

            var dbList = await productAndGiftCardMaps.ToListAsync();
            var results = new List<GetProductAndGiftCardMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductAndGiftCardMapForViewDto()
                {
                    ProductAndGiftCardMap = new ProductAndGiftCardMapDto
                    {

                        PurchaseAmount = o.PurchaseAmount,
                        GiftAmount = o.GiftAmount,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    CurrencyName = o.CurrencyName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductAndGiftCardMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductAndGiftCardMapForViewDto> GetProductAndGiftCardMapForView(long id)
        {
            var productAndGiftCardMap = await _productAndGiftCardMapRepository.GetAsync(id);

            var output = new GetProductAndGiftCardMapForViewDto { ProductAndGiftCardMap = ObjectMapper.Map<ProductAndGiftCardMapDto>(productAndGiftCardMap) };

            if (output.ProductAndGiftCardMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductAndGiftCardMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductAndGiftCardMap.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.ProductAndGiftCardMap.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductAndGiftCardMaps_Edit)]
        public async Task<GetProductAndGiftCardMapForEditOutput> GetProductAndGiftCardMapForEdit(EntityDto<long> input)
        {
            var productAndGiftCardMap = await _productAndGiftCardMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductAndGiftCardMapForEditOutput { ProductAndGiftCardMap = ObjectMapper.Map<CreateOrEditProductAndGiftCardMapDto>(productAndGiftCardMap) };

            if (output.ProductAndGiftCardMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductAndGiftCardMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductAndGiftCardMap.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.ProductAndGiftCardMap.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductAndGiftCardMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductAndGiftCardMaps_Create)]
        protected virtual async Task Create(CreateOrEditProductAndGiftCardMapDto input)
        {
            var productAndGiftCardMap = ObjectMapper.Map<ProductAndGiftCardMap>(input);

            if (AbpSession.TenantId != null)
            {
                productAndGiftCardMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _productAndGiftCardMapRepository.InsertAsync(productAndGiftCardMap);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductAndGiftCardMaps_Edit)]
        protected virtual async Task Update(CreateOrEditProductAndGiftCardMapDto input)
        {
            var productAndGiftCardMap = await _productAndGiftCardMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productAndGiftCardMap);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductAndGiftCardMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productAndGiftCardMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductAndGiftCardMapsToExcel(GetAllProductAndGiftCardMapsForExcelInput input)
        {

            var filteredProductAndGiftCardMaps = _productAndGiftCardMapRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.CurrencyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinPurchaseAmountFilter != null, e => e.PurchaseAmount >= input.MinPurchaseAmountFilter)
                        .WhereIf(input.MaxPurchaseAmountFilter != null, e => e.PurchaseAmount <= input.MaxPurchaseAmountFilter)
                        .WhereIf(input.MinGiftAmountFilter != null, e => e.GiftAmount >= input.MinGiftAmountFilter)
                        .WhereIf(input.MaxGiftAmountFilter != null, e => e.GiftAmount <= input.MaxGiftAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter);

            var query = (from o in filteredProductAndGiftCardMaps
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProductAndGiftCardMapForViewDto()
                         {
                             ProductAndGiftCardMap = new ProductAndGiftCardMapDto
                             {
                                 PurchaseAmount = o.PurchaseAmount,
                                 GiftAmount = o.GiftAmount,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             CurrencyName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var productAndGiftCardMapListDtos = await query.ToListAsync();

            return _productAndGiftCardMapsExcelExporter.ExportToFile(productAndGiftCardMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductAndGiftCardMaps)]
        public async Task<PagedResultDto<ProductAndGiftCardMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductAndGiftCardMapProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductAndGiftCardMapProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductAndGiftCardMapProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductAndGiftCardMaps)]
        public async Task<PagedResultDto<ProductAndGiftCardMapCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_currencyRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var currencyList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductAndGiftCardMapCurrencyLookupTableDto>();
            foreach (var currency in currencyList)
            {
                lookupTableDtoList.Add(new ProductAndGiftCardMapCurrencyLookupTableDto
                {
                    Id = currency.Id,
                    DisplayName = currency.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductAndGiftCardMapCurrencyLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}