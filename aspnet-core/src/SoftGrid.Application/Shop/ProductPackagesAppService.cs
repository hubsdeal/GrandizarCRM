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
    [AbpAuthorize(AppPermissions.Pages_ProductPackages)]
    public class ProductPackagesAppService : SoftGridAppServiceBase, IProductPackagesAppService
    {
        private readonly IRepository<ProductPackage, long> _productPackageRepository;
        private readonly IProductPackagesExcelExporter _productPackagesExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<MediaLibrary, long> _lookup_mediaLibraryRepository;

        public ProductPackagesAppService(IRepository<ProductPackage, long> productPackageRepository, IProductPackagesExcelExporter productPackagesExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<MediaLibrary, long> lookup_mediaLibraryRepository)
        {
            _productPackageRepository = productPackageRepository;
            _productPackagesExcelExporter = productPackagesExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_mediaLibraryRepository = lookup_mediaLibraryRepository;

        }

        public async Task<PagedResultDto<GetProductPackageForViewDto>> GetAll(GetAllProductPackagesInput input)
        {

            var filteredProductPackages = _productPackageRepository.GetAll()
                        .Include(e => e.PrimaryProductFk)
                        .Include(e => e.MediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinPackageProductIdFilter != null, e => e.PackageProductId >= input.MinPackageProductIdFilter)
                        .WhereIf(input.MaxPackageProductIdFilter != null, e => e.PackageProductId <= input.MaxPackageProductIdFilter)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.PrimaryProductFk != null && e.PrimaryProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.MediaLibraryFk != null && e.MediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var pagedAndFilteredProductPackages = filteredProductPackages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productPackages = from o in pagedAndFilteredProductPackages
                                  join o1 in _lookup_productRepository.GetAll() on o.PrimaryProductId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  select new
                                  {

                                      o.PackageProductId,
                                      o.DisplaySequence,
                                      o.Price,
                                      o.Quantity,
                                      Id = o.Id,
                                      ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                      MediaLibraryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                  };

            var totalCount = await filteredProductPackages.CountAsync();

            var dbList = await productPackages.ToListAsync();
            var results = new List<GetProductPackageForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductPackageForViewDto()
                {
                    ProductPackage = new ProductPackageDto
                    {

                        PackageProductId = o.PackageProductId,
                        DisplaySequence = o.DisplaySequence,
                        Price = o.Price,
                        Quantity = o.Quantity,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    MediaLibraryName = o.MediaLibraryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductPackageForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductPackageForViewDto> GetProductPackageForView(long id)
        {
            var productPackage = await _productPackageRepository.GetAsync(id);

            var output = new GetProductPackageForViewDto { ProductPackage = ObjectMapper.Map<ProductPackageDto>(productPackage) };

            if (output.ProductPackage.PrimaryProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductPackage.PrimaryProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductPackage.MediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.ProductPackage.MediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductPackages_Edit)]
        public async Task<GetProductPackageForEditOutput> GetProductPackageForEdit(EntityDto<long> input)
        {
            var productPackage = await _productPackageRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductPackageForEditOutput { ProductPackage = ObjectMapper.Map<CreateOrEditProductPackageDto>(productPackage) };

            if (output.ProductPackage.PrimaryProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductPackage.PrimaryProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductPackage.MediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.ProductPackage.MediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductPackageDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductPackages_Create)]
        protected virtual async Task Create(CreateOrEditProductPackageDto input)
        {
            var productPackage = ObjectMapper.Map<ProductPackage>(input);

            if (AbpSession.TenantId != null)
            {
                productPackage.TenantId = (int?)AbpSession.TenantId;
            }

            await _productPackageRepository.InsertAsync(productPackage);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductPackages_Edit)]
        protected virtual async Task Update(CreateOrEditProductPackageDto input)
        {
            var productPackage = await _productPackageRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productPackage);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductPackages_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productPackageRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductPackagesToExcel(GetAllProductPackagesForExcelInput input)
        {

            var filteredProductPackages = _productPackageRepository.GetAll()
                        .Include(e => e.PrimaryProductFk)
                        .Include(e => e.MediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinPackageProductIdFilter != null, e => e.PackageProductId >= input.MinPackageProductIdFilter)
                        .WhereIf(input.MaxPackageProductIdFilter != null, e => e.PackageProductId <= input.MaxPackageProductIdFilter)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.PrimaryProductFk != null && e.PrimaryProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.MediaLibraryFk != null && e.MediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var query = (from o in filteredProductPackages
                         join o1 in _lookup_productRepository.GetAll() on o.PrimaryProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProductPackageForViewDto()
                         {
                             ProductPackage = new ProductPackageDto
                             {
                                 PackageProductId = o.PackageProductId,
                                 DisplaySequence = o.DisplaySequence,
                                 Price = o.Price,
                                 Quantity = o.Quantity,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MediaLibraryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var productPackageListDtos = await query.ToListAsync();

            return _productPackagesExcelExporter.ExportToFile(productPackageListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductPackages)]
        public async Task<PagedResultDto<ProductPackageProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductPackageProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductPackageProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductPackageProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductPackages)]
        public async Task<PagedResultDto<ProductPackageMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_mediaLibraryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var mediaLibraryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductPackageMediaLibraryLookupTableDto>();
            foreach (var mediaLibrary in mediaLibraryList)
            {
                lookupTableDtoList.Add(new ProductPackageMediaLibraryLookupTableDto
                {
                    Id = mediaLibrary.Id,
                    DisplayName = mediaLibrary.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductPackageMediaLibraryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}