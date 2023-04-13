using SoftGrid.Shop;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.CRM;
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
    [AbpAuthorize(AppPermissions.Pages_Products)]
    public class ProductsAppService : SoftGridAppServiceBase, IProductsAppService
    {
        private readonly IRepository<Product, long> _productRepository;
        private readonly IProductsExcelExporter _productsExcelExporter;
        private readonly IRepository<ProductCategory, long> _lookup_productCategoryRepository;
        private readonly IRepository<MediaLibrary, long> _lookup_mediaLibraryRepository;
        private readonly IRepository<MeasurementUnit, long> _lookup_measurementUnitRepository;
        private readonly IRepository<Currency, long> _lookup_currencyRepository;
        private readonly IRepository<RatingLike, long> _lookup_ratingLikeRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public ProductsAppService(IRepository<Product, long> productRepository, IBinaryObjectManager binaryObjectManager, IProductsExcelExporter productsExcelExporter, IRepository<ProductCategory, long> lookup_productCategoryRepository, IRepository<MediaLibrary, long> lookup_mediaLibraryRepository, IRepository<MeasurementUnit, long> lookup_measurementUnitRepository, IRepository<Currency, long> lookup_currencyRepository, IRepository<RatingLike, long> lookup_ratingLikeRepository, IRepository<Contact, long> lookup_contactRepository, IRepository<Store, long> lookup_storeRepository)
        {
            _productRepository = productRepository;
            _productsExcelExporter = productsExcelExporter;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;
            _lookup_mediaLibraryRepository = lookup_mediaLibraryRepository;
            _lookup_measurementUnitRepository = lookup_measurementUnitRepository;
            _lookup_currencyRepository = lookup_currencyRepository;
            _lookup_ratingLikeRepository = lookup_ratingLikeRepository;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_storeRepository = lookup_storeRepository;
            _binaryObjectManager = binaryObjectManager;
        }

        public async Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input)
        {

            var filteredProducts = _productRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.MediaLibraryFk)
                        .Include(e => e.MeasurementUnitFk)
                        .Include(e => e.CurrencyFk)
                        .Include(e => e.RatingLikeFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.ShortDescription.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Sku.Contains(input.Filter) || e.Url.Contains(input.Filter) || e.SeoTitle.Contains(input.Filter) || e.MetaKeywords.Contains(input.Filter) || e.MetaDescription.Contains(input.Filter) || e.InternalNotes.Contains(input.Filter) || e.ProductManufacturerSku.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ShortDescriptionFilter), e => e.ShortDescription.Contains(input.ShortDescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SkuFilter), e => e.Sku.Contains(input.SkuFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter), e => e.Url.Contains(input.UrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SeoTitleFilter), e => e.SeoTitle.Contains(input.SeoTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaKeywordsFilter), e => e.MetaKeywords.Contains(input.MetaKeywordsFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaDescriptionFilter), e => e.MetaDescription.Contains(input.MetaDescriptionFilter))
                        .WhereIf(input.MinRegularPriceFilter != null, e => e.RegularPrice >= input.MinRegularPriceFilter)
                        .WhereIf(input.MaxRegularPriceFilter != null, e => e.RegularPrice <= input.MaxRegularPriceFilter)
                        .WhereIf(input.MinSalePriceFilter != null, e => e.SalePrice >= input.MinSalePriceFilter)
                        .WhereIf(input.MaxSalePriceFilter != null, e => e.SalePrice <= input.MaxSalePriceFilter)
                        .WhereIf(input.MinPriceDiscountPercentageFilter != null, e => e.PriceDiscountPercentage >= input.MinPriceDiscountPercentageFilter)
                        .WhereIf(input.MaxPriceDiscountPercentageFilter != null, e => e.PriceDiscountPercentage <= input.MaxPriceDiscountPercentageFilter)
                        .WhereIf(input.CallForPriceFilter.HasValue && input.CallForPriceFilter > -1, e => (input.CallForPriceFilter == 1 && e.CallForPrice) || (input.CallForPriceFilter == 0 && !e.CallForPrice))
                        .WhereIf(input.MinUnitPriceFilter != null, e => e.UnitPrice >= input.MinUnitPriceFilter)
                        .WhereIf(input.MaxUnitPriceFilter != null, e => e.UnitPrice <= input.MaxUnitPriceFilter)
                        .WhereIf(input.MinMeasurementAmountFilter != null, e => e.MeasurementAmount >= input.MinMeasurementAmountFilter)
                        .WhereIf(input.MaxMeasurementAmountFilter != null, e => e.MeasurementAmount <= input.MaxMeasurementAmountFilter)
                        .WhereIf(input.IsTaxExemptFilter.HasValue && input.IsTaxExemptFilter > -1, e => (input.IsTaxExemptFilter == 1 && e.IsTaxExempt) || (input.IsTaxExemptFilter == 0 && !e.IsTaxExempt))
                        .WhereIf(input.MinStockQuantityFilter != null, e => e.StockQuantity >= input.MinStockQuantityFilter)
                        .WhereIf(input.MaxStockQuantityFilter != null, e => e.StockQuantity <= input.MaxStockQuantityFilter)
                        .WhereIf(input.IsDisplayStockQuantityFilter.HasValue && input.IsDisplayStockQuantityFilter > -1, e => (input.IsDisplayStockQuantityFilter == 1 && e.IsDisplayStockQuantity) || (input.IsDisplayStockQuantityFilter == 0 && !e.IsDisplayStockQuantity))
                        .WhereIf(input.IsPublishedFilter.HasValue && input.IsPublishedFilter > -1, e => (input.IsPublishedFilter == 1 && e.IsPublished) || (input.IsPublishedFilter == 0 && !e.IsPublished))
                        .WhereIf(input.IsPackageProductFilter.HasValue && input.IsPackageProductFilter > -1, e => (input.IsPackageProductFilter == 1 && e.IsPackageProduct) || (input.IsPackageProductFilter == 0 && !e.IsPackageProduct))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InternalNotesFilter), e => e.InternalNotes.Contains(input.InternalNotesFilter))
                        .WhereIf(input.IsTemplateFilter.HasValue && input.IsTemplateFilter > -1, e => (input.IsTemplateFilter == 1 && e.IsTemplate) || (input.IsTemplateFilter == 0 && !e.IsTemplate))
                        .WhereIf(input.MinPriceDiscountAmountFilter != null, e => e.PriceDiscountAmount >= input.MinPriceDiscountAmountFilter)
                        .WhereIf(input.MaxPriceDiscountAmountFilter != null, e => e.PriceDiscountAmount <= input.MaxPriceDiscountAmountFilter)
                        .WhereIf(input.IsServiceFilter.HasValue && input.IsServiceFilter > -1, e => (input.IsServiceFilter == 1 && e.IsService) || (input.IsServiceFilter == 0 && !e.IsService))
                        .WhereIf(input.IsWholeSaleProductFilter.HasValue && input.IsWholeSaleProductFilter > -1, e => (input.IsWholeSaleProductFilter == 1 && e.IsWholeSaleProduct) || (input.IsWholeSaleProductFilter == 0 && !e.IsWholeSaleProduct))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductManufacturerSkuFilter), e => e.ProductManufacturerSku.Contains(input.ProductManufacturerSkuFilter))
                        .WhereIf(input.ByOrderOnlyFilter.HasValue && input.ByOrderOnlyFilter > -1, e => (input.ByOrderOnlyFilter == 1 && e.ByOrderOnly) || (input.ByOrderOnlyFilter == 0 && !e.ByOrderOnly))
                        .WhereIf(input.MinScoreFilter != null, e => e.Score >= input.MinScoreFilter)
                        .WhereIf(input.MaxScoreFilter != null, e => e.Score <= input.MaxScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.MediaLibraryFk != null && e.MediaLibraryFk.Name == input.MediaLibraryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MeasurementUnitNameFilter), e => e.MeasurementUnitFk != null && e.MeasurementUnitFk.Name == input.MeasurementUnitNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RatingLikeNameFilter), e => e.RatingLikeFk != null && e.RatingLikeFk.Name == input.RatingLikeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var pagedAndFilteredProducts = filteredProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var products = from o in pagedAndFilteredProducts
                           join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o2 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           join o3 in _lookup_measurementUnitRepository.GetAll() on o.MeasurementUnitId equals o3.Id into j3
                           from s3 in j3.DefaultIfEmpty()

                           join o4 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o4.Id into j4
                           from s4 in j4.DefaultIfEmpty()

                           join o5 in _lookup_ratingLikeRepository.GetAll() on o.RatingLikeId equals o5.Id into j5
                           from s5 in j5.DefaultIfEmpty()

                           join o6 in _lookup_contactRepository.GetAll() on o.ContactId equals o6.Id into j6
                           from s6 in j6.DefaultIfEmpty()

                           join o7 in _lookup_storeRepository.GetAll() on o.StoreId equals o7.Id into j7
                           from s7 in j7.DefaultIfEmpty()

                           select new
                           {

                               o.Name,
                               o.ShortDescription,
                               o.Description,
                               o.Sku,
                               o.Url,
                               o.SeoTitle,
                               o.MetaKeywords,
                               o.MetaDescription,
                               o.RegularPrice,
                               o.SalePrice,
                               o.PriceDiscountPercentage,
                               o.CallForPrice,
                               o.UnitPrice,
                               o.MeasurementAmount,
                               o.IsTaxExempt,
                               o.StockQuantity,
                               o.IsDisplayStockQuantity,
                               o.IsPublished,
                               o.IsPackageProduct,
                               o.InternalNotes,
                               o.IsTemplate,
                               o.PriceDiscountAmount,
                               o.IsService,
                               o.IsWholeSaleProduct,
                               o.ProductManufacturerSku,
                               o.ByOrderOnly,
                               o.Score,
                               Id = o.Id,
                               PictureId = s2==null?Guid.Empty:s2.BinaryObjectId,
                               ProductCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                               MediaLibraryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                               MeasurementUnitName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                               CurrencyName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                               RatingLikeName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                               ContactFullName = s6 == null || s6.FullName == null ? "" : s6.FullName.ToString(),
                               StoreName = s7 == null || s7.Name == null ? "" : s7.Name.ToString()
                           };

            var totalCount = await filteredProducts.CountAsync();

            var dbList = await products.ToListAsync();
            var results = new List<GetProductForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductForViewDto()
                {
                    Product = new ProductDto
                    {

                        Name = o.Name,
                        ShortDescription = o.ShortDescription,
                        Description = o.Description,
                        Sku = o.Sku,
                        Url = o.Url,
                        SeoTitle = o.SeoTitle,
                        MetaKeywords = o.MetaKeywords,
                        MetaDescription = o.MetaDescription,
                        RegularPrice = o.RegularPrice,
                        SalePrice = o.SalePrice,
                        PriceDiscountPercentage = o.PriceDiscountPercentage,
                        CallForPrice = o.CallForPrice,
                        UnitPrice = o.UnitPrice,
                        MeasurementAmount = o.MeasurementAmount,
                        IsTaxExempt = o.IsTaxExempt,
                        StockQuantity = o.StockQuantity,
                        IsDisplayStockQuantity = o.IsDisplayStockQuantity,
                        IsPublished = o.IsPublished,
                        IsPackageProduct = o.IsPackageProduct,
                        InternalNotes = o.InternalNotes,
                        IsTemplate = o.IsTemplate,
                        PriceDiscountAmount = o.PriceDiscountAmount,
                        IsService = o.IsService,
                        IsWholeSaleProduct = o.IsWholeSaleProduct,
                        ProductManufacturerSku = o.ProductManufacturerSku,
                        ByOrderOnly = o.ByOrderOnly,
                        Score = o.Score,
                        Id = o.Id,
                        PictureId=o.PictureId
                    },
                    ProductCategoryName = o.ProductCategoryName,
                    MediaLibraryName = o.MediaLibraryName,
                    MeasurementUnitName = o.MeasurementUnitName,
                    CurrencyName = o.CurrencyName,
                    RatingLikeName = o.RatingLikeName,
                    ContactFullName = o.ContactFullName,
                    StoreName = o.StoreName
                };
                if (res.Product.PictureId != null && res.Product.PictureId != Guid.Empty)
                {
                    res.Product.Picture = await _binaryObjectManager.GetOthersPictureUrlAsync((Guid)o.PictureId, ".png");
                }

                results.Add(res);
            }

            return new PagedResultDto<GetProductForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductForViewDto> GetProductForView(long id)
        {
            var product = await _productRepository.GetAsync(id);

            var output = new GetProductForViewDto { Product = ObjectMapper.Map<ProductDto>(product) };

            if (output.Product.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.Product.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.Product.MediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.Product.MediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            if (output.Product.MeasurementUnitId != null)
            {
                var _lookupMeasurementUnit = await _lookup_measurementUnitRepository.FirstOrDefaultAsync((long)output.Product.MeasurementUnitId);
                output.MeasurementUnitName = _lookupMeasurementUnit?.Name?.ToString();
            }

            if (output.Product.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.Product.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            if (output.Product.RatingLikeId != null)
            {
                var _lookupRatingLike = await _lookup_ratingLikeRepository.FirstOrDefaultAsync((long)output.Product.RatingLikeId);
                output.RatingLikeName = _lookupRatingLike?.Name?.ToString();
            }

            if (output.Product.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.Product.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.Product.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.Product.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Products_Edit)]
        public async Task<GetProductForEditOutput> GetProductForEdit(EntityDto<long> input)
        {
            var product = await _productRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductForEditOutput { Product = ObjectMapper.Map<CreateOrEditProductDto>(product) };

            if (output.Product.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.Product.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.Product.MediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.Product.MediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            if (output.Product.MeasurementUnitId != null)
            {
                var _lookupMeasurementUnit = await _lookup_measurementUnitRepository.FirstOrDefaultAsync((long)output.Product.MeasurementUnitId);
                output.MeasurementUnitName = _lookupMeasurementUnit?.Name?.ToString();
            }

            if (output.Product.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.Product.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            if (output.Product.RatingLikeId != null)
            {
                var _lookupRatingLike = await _lookup_ratingLikeRepository.FirstOrDefaultAsync((long)output.Product.RatingLikeId);
                output.RatingLikeName = _lookupRatingLike?.Name?.ToString();
            }

            if (output.Product.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.Product.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.Product.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.Product.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Products_Create)]
        protected virtual async Task Create(CreateOrEditProductDto input)
        {
            var product = ObjectMapper.Map<Product>(input);

            if (AbpSession.TenantId != null)
            {
                product.TenantId = (int?)AbpSession.TenantId;
            }

            await _productRepository.InsertAsync(product);

        }

        [AbpAuthorize(AppPermissions.Pages_Products_Edit)]
        protected virtual async Task Update(CreateOrEditProductDto input)
        {
            var product = await _productRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, product);

        }

        [AbpAuthorize(AppPermissions.Pages_Products_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input)
        {

            var filteredProducts = _productRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.MediaLibraryFk)
                        .Include(e => e.MeasurementUnitFk)
                        .Include(e => e.CurrencyFk)
                        .Include(e => e.RatingLikeFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.ShortDescription.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Sku.Contains(input.Filter) || e.Url.Contains(input.Filter) || e.SeoTitle.Contains(input.Filter) || e.MetaKeywords.Contains(input.Filter) || e.MetaDescription.Contains(input.Filter) || e.InternalNotes.Contains(input.Filter) || e.ProductManufacturerSku.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ShortDescriptionFilter), e => e.ShortDescription.Contains(input.ShortDescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SkuFilter), e => e.Sku.Contains(input.SkuFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter), e => e.Url.Contains(input.UrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SeoTitleFilter), e => e.SeoTitle.Contains(input.SeoTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaKeywordsFilter), e => e.MetaKeywords.Contains(input.MetaKeywordsFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaDescriptionFilter), e => e.MetaDescription.Contains(input.MetaDescriptionFilter))
                        .WhereIf(input.MinRegularPriceFilter != null, e => e.RegularPrice >= input.MinRegularPriceFilter)
                        .WhereIf(input.MaxRegularPriceFilter != null, e => e.RegularPrice <= input.MaxRegularPriceFilter)
                        .WhereIf(input.MinSalePriceFilter != null, e => e.SalePrice >= input.MinSalePriceFilter)
                        .WhereIf(input.MaxSalePriceFilter != null, e => e.SalePrice <= input.MaxSalePriceFilter)
                        .WhereIf(input.MinPriceDiscountPercentageFilter != null, e => e.PriceDiscountPercentage >= input.MinPriceDiscountPercentageFilter)
                        .WhereIf(input.MaxPriceDiscountPercentageFilter != null, e => e.PriceDiscountPercentage <= input.MaxPriceDiscountPercentageFilter)
                        .WhereIf(input.CallForPriceFilter.HasValue && input.CallForPriceFilter > -1, e => (input.CallForPriceFilter == 1 && e.CallForPrice) || (input.CallForPriceFilter == 0 && !e.CallForPrice))
                        .WhereIf(input.MinUnitPriceFilter != null, e => e.UnitPrice >= input.MinUnitPriceFilter)
                        .WhereIf(input.MaxUnitPriceFilter != null, e => e.UnitPrice <= input.MaxUnitPriceFilter)
                        .WhereIf(input.MinMeasurementAmountFilter != null, e => e.MeasurementAmount >= input.MinMeasurementAmountFilter)
                        .WhereIf(input.MaxMeasurementAmountFilter != null, e => e.MeasurementAmount <= input.MaxMeasurementAmountFilter)
                        .WhereIf(input.IsTaxExemptFilter.HasValue && input.IsTaxExemptFilter > -1, e => (input.IsTaxExemptFilter == 1 && e.IsTaxExempt) || (input.IsTaxExemptFilter == 0 && !e.IsTaxExempt))
                        .WhereIf(input.MinStockQuantityFilter != null, e => e.StockQuantity >= input.MinStockQuantityFilter)
                        .WhereIf(input.MaxStockQuantityFilter != null, e => e.StockQuantity <= input.MaxStockQuantityFilter)
                        .WhereIf(input.IsDisplayStockQuantityFilter.HasValue && input.IsDisplayStockQuantityFilter > -1, e => (input.IsDisplayStockQuantityFilter == 1 && e.IsDisplayStockQuantity) || (input.IsDisplayStockQuantityFilter == 0 && !e.IsDisplayStockQuantity))
                        .WhereIf(input.IsPublishedFilter.HasValue && input.IsPublishedFilter > -1, e => (input.IsPublishedFilter == 1 && e.IsPublished) || (input.IsPublishedFilter == 0 && !e.IsPublished))
                        .WhereIf(input.IsPackageProductFilter.HasValue && input.IsPackageProductFilter > -1, e => (input.IsPackageProductFilter == 1 && e.IsPackageProduct) || (input.IsPackageProductFilter == 0 && !e.IsPackageProduct))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InternalNotesFilter), e => e.InternalNotes.Contains(input.InternalNotesFilter))
                        .WhereIf(input.IsTemplateFilter.HasValue && input.IsTemplateFilter > -1, e => (input.IsTemplateFilter == 1 && e.IsTemplate) || (input.IsTemplateFilter == 0 && !e.IsTemplate))
                        .WhereIf(input.MinPriceDiscountAmountFilter != null, e => e.PriceDiscountAmount >= input.MinPriceDiscountAmountFilter)
                        .WhereIf(input.MaxPriceDiscountAmountFilter != null, e => e.PriceDiscountAmount <= input.MaxPriceDiscountAmountFilter)
                        .WhereIf(input.IsServiceFilter.HasValue && input.IsServiceFilter > -1, e => (input.IsServiceFilter == 1 && e.IsService) || (input.IsServiceFilter == 0 && !e.IsService))
                        .WhereIf(input.IsWholeSaleProductFilter.HasValue && input.IsWholeSaleProductFilter > -1, e => (input.IsWholeSaleProductFilter == 1 && e.IsWholeSaleProduct) || (input.IsWholeSaleProductFilter == 0 && !e.IsWholeSaleProduct))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductManufacturerSkuFilter), e => e.ProductManufacturerSku.Contains(input.ProductManufacturerSkuFilter))
                        .WhereIf(input.ByOrderOnlyFilter.HasValue && input.ByOrderOnlyFilter > -1, e => (input.ByOrderOnlyFilter == 1 && e.ByOrderOnly) || (input.ByOrderOnlyFilter == 0 && !e.ByOrderOnly))
                        .WhereIf(input.MinScoreFilter != null, e => e.Score >= input.MinScoreFilter)
                        .WhereIf(input.MaxScoreFilter != null, e => e.Score <= input.MaxScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.MediaLibraryFk != null && e.MediaLibraryFk.Name == input.MediaLibraryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MeasurementUnitNameFilter), e => e.MeasurementUnitFk != null && e.MeasurementUnitFk.Name == input.MeasurementUnitNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RatingLikeNameFilter), e => e.RatingLikeFk != null && e.RatingLikeFk.Name == input.RatingLikeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var query = (from o in filteredProducts
                         join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_measurementUnitRepository.GetAll() on o.MeasurementUnitId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_ratingLikeRepository.GetAll() on o.RatingLikeId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         join o6 in _lookup_contactRepository.GetAll() on o.ContactId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()

                         join o7 in _lookup_storeRepository.GetAll() on o.StoreId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()

                         select new GetProductForViewDto()
                         {
                             Product = new ProductDto
                             {
                                 Name = o.Name,
                                 ShortDescription = o.ShortDescription,
                                 Description = o.Description,
                                 Sku = o.Sku,
                                 Url = o.Url,
                                 SeoTitle = o.SeoTitle,
                                 MetaKeywords = o.MetaKeywords,
                                 MetaDescription = o.MetaDescription,
                                 RegularPrice = o.RegularPrice,
                                 SalePrice = o.SalePrice,
                                 PriceDiscountPercentage = o.PriceDiscountPercentage,
                                 CallForPrice = o.CallForPrice,
                                 UnitPrice = o.UnitPrice,
                                 MeasurementAmount = o.MeasurementAmount,
                                 IsTaxExempt = o.IsTaxExempt,
                                 StockQuantity = o.StockQuantity,
                                 IsDisplayStockQuantity = o.IsDisplayStockQuantity,
                                 IsPublished = o.IsPublished,
                                 IsPackageProduct = o.IsPackageProduct,
                                 InternalNotes = o.InternalNotes,
                                 IsTemplate = o.IsTemplate,
                                 PriceDiscountAmount = o.PriceDiscountAmount,
                                 IsService = o.IsService,
                                 IsWholeSaleProduct = o.IsWholeSaleProduct,
                                 ProductManufacturerSku = o.ProductManufacturerSku,
                                 ByOrderOnly = o.ByOrderOnly,
                                 Score = o.Score,
                                 Id = o.Id
                             },
                             ProductCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MediaLibraryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             MeasurementUnitName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             CurrencyName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                             RatingLikeName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                             ContactFullName = s6 == null || s6.FullName == null ? "" : s6.FullName.ToString(),
                             StoreName = s7 == null || s7.Name == null ? "" : s7.Name.ToString()
                         });

            var productListDtos = await query.ToListAsync();

            return _productsExcelExporter.ExportToFile(productListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Products)]
        public async Task<PagedResultDto<ProductProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductProductCategoryLookupTableDto>();
            foreach (var productCategory in productCategoryList)
            {
                lookupTableDtoList.Add(new ProductProductCategoryLookupTableDto
                {
                    Id = productCategory.Id,
                    DisplayName = productCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Products)]
        public async Task<PagedResultDto<ProductMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_mediaLibraryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var mediaLibraryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductMediaLibraryLookupTableDto>();
            foreach (var mediaLibrary in mediaLibraryList)
            {
                lookupTableDtoList.Add(new ProductMediaLibraryLookupTableDto
                {
                    Id = mediaLibrary.Id,
                    DisplayName = mediaLibrary.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductMediaLibraryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Products)]
        public async Task<PagedResultDto<ProductMeasurementUnitLookupTableDto>> GetAllMeasurementUnitForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_measurementUnitRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var measurementUnitList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductMeasurementUnitLookupTableDto>();
            foreach (var measurementUnit in measurementUnitList)
            {
                lookupTableDtoList.Add(new ProductMeasurementUnitLookupTableDto
                {
                    Id = measurementUnit.Id,
                    DisplayName = measurementUnit.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductMeasurementUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Products)]
        public async Task<PagedResultDto<ProductCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_currencyRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var currencyList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCurrencyLookupTableDto>();
            foreach (var currency in currencyList)
            {
                lookupTableDtoList.Add(new ProductCurrencyLookupTableDto
                {
                    Id = currency.Id,
                    DisplayName = currency.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductCurrencyLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Products)]
        public async Task<PagedResultDto<ProductRatingLikeLookupTableDto>> GetAllRatingLikeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_ratingLikeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var ratingLikeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductRatingLikeLookupTableDto>();
            foreach (var ratingLike in ratingLikeList)
            {
                lookupTableDtoList.Add(new ProductRatingLikeLookupTableDto
                {
                    Id = ratingLike.Id,
                    DisplayName = ratingLike.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductRatingLikeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Products)]
        public async Task<PagedResultDto<ProductContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new ProductContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<ProductContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Products)]
        public async Task<PagedResultDto<ProductStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new ProductStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}