using SoftGrid.Shop;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
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
    [AbpAuthorize(AppPermissions.Pages_ProductWholeSalePrices)]
    public class ProductWholeSalePricesAppService : SoftGridAppServiceBase, IProductWholeSalePricesAppService
    {
        private readonly IRepository<ProductWholeSalePrice, long> _productWholeSalePriceRepository;
        private readonly IProductWholeSalePricesExcelExporter _productWholeSalePricesExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<ProductWholeSaleQuantityType, long> _lookup_productWholeSaleQuantityTypeRepository;
        private readonly IRepository<MeasurementUnit, long> _lookup_measurementUnitRepository;
        private readonly IRepository<Currency, long> _lookup_currencyRepository;

        public ProductWholeSalePricesAppService(IRepository<ProductWholeSalePrice, long> productWholeSalePriceRepository, IProductWholeSalePricesExcelExporter productWholeSalePricesExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<ProductWholeSaleQuantityType, long> lookup_productWholeSaleQuantityTypeRepository, IRepository<MeasurementUnit, long> lookup_measurementUnitRepository, IRepository<Currency, long> lookup_currencyRepository)
        {
            _productWholeSalePriceRepository = productWholeSalePriceRepository;
            _productWholeSalePricesExcelExporter = productWholeSalePricesExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_productWholeSaleQuantityTypeRepository = lookup_productWholeSaleQuantityTypeRepository;
            _lookup_measurementUnitRepository = lookup_measurementUnitRepository;
            _lookup_currencyRepository = lookup_currencyRepository;

        }

        public async Task<PagedResultDto<GetProductWholeSalePriceForViewDto>> GetAll(GetAllProductWholeSalePricesInput input)
        {

            var filteredProductWholeSalePrices = _productWholeSalePriceRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ProductWholeSaleQuantityTypeFk)
                        .Include(e => e.MeasurementUnitFk)
                        .Include(e => e.CurrencyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PackageInfo.Contains(input.Filter) || e.WholeSaleSkuId.Contains(input.Filter))
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinExactQuantityFilter != null, e => e.ExactQuantity >= input.MinExactQuantityFilter)
                        .WhereIf(input.MaxExactQuantityFilter != null, e => e.ExactQuantity <= input.MaxExactQuantityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PackageInfoFilter), e => e.PackageInfo.Contains(input.PackageInfoFilter))
                        .WhereIf(input.MinPackageQuantityFilter != null, e => e.PackageQuantity >= input.MinPackageQuantityFilter)
                        .WhereIf(input.MaxPackageQuantityFilter != null, e => e.PackageQuantity <= input.MaxPackageQuantityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WholeSaleSkuIdFilter), e => e.WholeSaleSkuId.Contains(input.WholeSaleSkuIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductWholeSaleQuantityTypeNameFilter), e => e.ProductWholeSaleQuantityTypeFk != null && e.ProductWholeSaleQuantityTypeFk.Name == input.ProductWholeSaleQuantityTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MeasurementUnitNameFilter), e => e.MeasurementUnitFk != null && e.MeasurementUnitFk.Name == input.MeasurementUnitNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter);

            var pagedAndFilteredProductWholeSalePrices = filteredProductWholeSalePrices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productWholeSalePrices = from o in pagedAndFilteredProductWholeSalePrices
                                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         join o2 in _lookup_productWholeSaleQuantityTypeRepository.GetAll() on o.ProductWholeSaleQuantityTypeId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()

                                         join o3 in _lookup_measurementUnitRepository.GetAll() on o.MeasurementUnitId equals o3.Id into j3
                                         from s3 in j3.DefaultIfEmpty()

                                         join o4 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o4.Id into j4
                                         from s4 in j4.DefaultIfEmpty()

                                         select new
                                         {

                                             o.Price,
                                             o.ExactQuantity,
                                             o.PackageInfo,
                                             o.PackageQuantity,
                                             o.WholeSaleSkuId,
                                             Id = o.Id,
                                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                             ProductWholeSaleQuantityTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                             MeasurementUnitName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                             CurrencyName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                         };

            var totalCount = await filteredProductWholeSalePrices.CountAsync();

            var dbList = await productWholeSalePrices.ToListAsync();
            var results = new List<GetProductWholeSalePriceForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductWholeSalePriceForViewDto()
                {
                    ProductWholeSalePrice = new ProductWholeSalePriceDto
                    {

                        Price = o.Price,
                        ExactQuantity = o.ExactQuantity,
                        PackageInfo = o.PackageInfo,
                        PackageQuantity = o.PackageQuantity,
                        WholeSaleSkuId = o.WholeSaleSkuId,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    ProductWholeSaleQuantityTypeName = o.ProductWholeSaleQuantityTypeName,
                    MeasurementUnitName = o.MeasurementUnitName,
                    CurrencyName = o.CurrencyName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductWholeSalePriceForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductWholeSalePriceForViewDto> GetProductWholeSalePriceForView(long id)
        {
            var productWholeSalePrice = await _productWholeSalePriceRepository.GetAsync(id);

            var output = new GetProductWholeSalePriceForViewDto { ProductWholeSalePrice = ObjectMapper.Map<ProductWholeSalePriceDto>(productWholeSalePrice) };

            if (output.ProductWholeSalePrice.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductWholeSalePrice.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductWholeSalePrice.ProductWholeSaleQuantityTypeId != null)
            {
                var _lookupProductWholeSaleQuantityType = await _lookup_productWholeSaleQuantityTypeRepository.FirstOrDefaultAsync((long)output.ProductWholeSalePrice.ProductWholeSaleQuantityTypeId);
                output.ProductWholeSaleQuantityTypeName = _lookupProductWholeSaleQuantityType?.Name?.ToString();
            }

            if (output.ProductWholeSalePrice.MeasurementUnitId != null)
            {
                var _lookupMeasurementUnit = await _lookup_measurementUnitRepository.FirstOrDefaultAsync((long)output.ProductWholeSalePrice.MeasurementUnitId);
                output.MeasurementUnitName = _lookupMeasurementUnit?.Name?.ToString();
            }

            if (output.ProductWholeSalePrice.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.ProductWholeSalePrice.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductWholeSalePrices_Edit)]
        public async Task<GetProductWholeSalePriceForEditOutput> GetProductWholeSalePriceForEdit(EntityDto<long> input)
        {
            var productWholeSalePrice = await _productWholeSalePriceRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductWholeSalePriceForEditOutput { ProductWholeSalePrice = ObjectMapper.Map<CreateOrEditProductWholeSalePriceDto>(productWholeSalePrice) };

            if (output.ProductWholeSalePrice.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductWholeSalePrice.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductWholeSalePrice.ProductWholeSaleQuantityTypeId != null)
            {
                var _lookupProductWholeSaleQuantityType = await _lookup_productWholeSaleQuantityTypeRepository.FirstOrDefaultAsync((long)output.ProductWholeSalePrice.ProductWholeSaleQuantityTypeId);
                output.ProductWholeSaleQuantityTypeName = _lookupProductWholeSaleQuantityType?.Name?.ToString();
            }

            if (output.ProductWholeSalePrice.MeasurementUnitId != null)
            {
                var _lookupMeasurementUnit = await _lookup_measurementUnitRepository.FirstOrDefaultAsync((long)output.ProductWholeSalePrice.MeasurementUnitId);
                output.MeasurementUnitName = _lookupMeasurementUnit?.Name?.ToString();
            }

            if (output.ProductWholeSalePrice.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.ProductWholeSalePrice.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductWholeSalePriceDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductWholeSalePrices_Create)]
        protected virtual async Task Create(CreateOrEditProductWholeSalePriceDto input)
        {
            var productWholeSalePrice = ObjectMapper.Map<ProductWholeSalePrice>(input);

            if (AbpSession.TenantId != null)
            {
                productWholeSalePrice.TenantId = (int?)AbpSession.TenantId;
            }

            await _productWholeSalePriceRepository.InsertAsync(productWholeSalePrice);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductWholeSalePrices_Edit)]
        protected virtual async Task Update(CreateOrEditProductWholeSalePriceDto input)
        {
            var productWholeSalePrice = await _productWholeSalePriceRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productWholeSalePrice);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductWholeSalePrices_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productWholeSalePriceRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductWholeSalePricesToExcel(GetAllProductWholeSalePricesForExcelInput input)
        {

            var filteredProductWholeSalePrices = _productWholeSalePriceRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ProductWholeSaleQuantityTypeFk)
                        .Include(e => e.MeasurementUnitFk)
                        .Include(e => e.CurrencyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PackageInfo.Contains(input.Filter) || e.WholeSaleSkuId.Contains(input.Filter))
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinExactQuantityFilter != null, e => e.ExactQuantity >= input.MinExactQuantityFilter)
                        .WhereIf(input.MaxExactQuantityFilter != null, e => e.ExactQuantity <= input.MaxExactQuantityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PackageInfoFilter), e => e.PackageInfo.Contains(input.PackageInfoFilter))
                        .WhereIf(input.MinPackageQuantityFilter != null, e => e.PackageQuantity >= input.MinPackageQuantityFilter)
                        .WhereIf(input.MaxPackageQuantityFilter != null, e => e.PackageQuantity <= input.MaxPackageQuantityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WholeSaleSkuIdFilter), e => e.WholeSaleSkuId.Contains(input.WholeSaleSkuIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductWholeSaleQuantityTypeNameFilter), e => e.ProductWholeSaleQuantityTypeFk != null && e.ProductWholeSaleQuantityTypeFk.Name == input.ProductWholeSaleQuantityTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MeasurementUnitNameFilter), e => e.MeasurementUnitFk != null && e.MeasurementUnitFk.Name == input.MeasurementUnitNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter);

            var query = (from o in filteredProductWholeSalePrices
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productWholeSaleQuantityTypeRepository.GetAll() on o.ProductWholeSaleQuantityTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_measurementUnitRepository.GetAll() on o.MeasurementUnitId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetProductWholeSalePriceForViewDto()
                         {
                             ProductWholeSalePrice = new ProductWholeSalePriceDto
                             {
                                 Price = o.Price,
                                 ExactQuantity = o.ExactQuantity,
                                 PackageInfo = o.PackageInfo,
                                 PackageQuantity = o.PackageQuantity,
                                 WholeSaleSkuId = o.WholeSaleSkuId,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductWholeSaleQuantityTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             MeasurementUnitName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             CurrencyName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var productWholeSalePriceListDtos = await query.ToListAsync();

            return _productWholeSalePricesExcelExporter.ExportToFile(productWholeSalePriceListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductWholeSalePrices)]
        public async Task<PagedResultDto<ProductWholeSalePriceProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductWholeSalePriceProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductWholeSalePriceProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductWholeSalePriceProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductWholeSalePrices)]
        public async Task<PagedResultDto<ProductWholeSalePriceProductWholeSaleQuantityTypeLookupTableDto>> GetAllProductWholeSaleQuantityTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productWholeSaleQuantityTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productWholeSaleQuantityTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductWholeSalePriceProductWholeSaleQuantityTypeLookupTableDto>();
            foreach (var productWholeSaleQuantityType in productWholeSaleQuantityTypeList)
            {
                lookupTableDtoList.Add(new ProductWholeSalePriceProductWholeSaleQuantityTypeLookupTableDto
                {
                    Id = productWholeSaleQuantityType.Id,
                    DisplayName = productWholeSaleQuantityType.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductWholeSalePriceProductWholeSaleQuantityTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductWholeSalePrices)]
        public async Task<PagedResultDto<ProductWholeSalePriceMeasurementUnitLookupTableDto>> GetAllMeasurementUnitForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_measurementUnitRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var measurementUnitList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductWholeSalePriceMeasurementUnitLookupTableDto>();
            foreach (var measurementUnit in measurementUnitList)
            {
                lookupTableDtoList.Add(new ProductWholeSalePriceMeasurementUnitLookupTableDto
                {
                    Id = measurementUnit.Id,
                    DisplayName = measurementUnit.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductWholeSalePriceMeasurementUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductWholeSalePrices)]
        public async Task<PagedResultDto<ProductWholeSalePriceCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_currencyRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var currencyList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductWholeSalePriceCurrencyLookupTableDto>();
            foreach (var currency in currencyList)
            {
                lookupTableDtoList.Add(new ProductWholeSalePriceCurrencyLookupTableDto
                {
                    Id = currency.Id,
                    DisplayName = currency.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductWholeSalePriceCurrencyLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}