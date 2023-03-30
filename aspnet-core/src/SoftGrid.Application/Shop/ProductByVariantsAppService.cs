using SoftGrid.Shop;
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
    [AbpAuthorize(AppPermissions.Pages_ProductByVariants)]
    public class ProductByVariantsAppService : SoftGridAppServiceBase, IProductByVariantsAppService
    {
        private readonly IRepository<ProductByVariant, long> _productByVariantRepository;
        private readonly IProductByVariantsExcelExporter _productByVariantsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<ProductVariant, long> _lookup_productVariantRepository;
        private readonly IRepository<ProductVariantCategory, long> _lookup_productVariantCategoryRepository;
        private readonly IRepository<MediaLibrary, long> _lookup_mediaLibraryRepository;

        public ProductByVariantsAppService(IRepository<ProductByVariant, long> productByVariantRepository, IProductByVariantsExcelExporter productByVariantsExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<ProductVariant, long> lookup_productVariantRepository, IRepository<ProductVariantCategory, long> lookup_productVariantCategoryRepository, IRepository<MediaLibrary, long> lookup_mediaLibraryRepository)
        {
            _productByVariantRepository = productByVariantRepository;
            _productByVariantsExcelExporter = productByVariantsExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_productVariantRepository = lookup_productVariantRepository;
            _lookup_productVariantCategoryRepository = lookup_productVariantCategoryRepository;
            _lookup_mediaLibraryRepository = lookup_mediaLibraryRepository;

        }

        public async Task<PagedResultDto<GetProductByVariantForViewDto>> GetAll(GetAllProductByVariantsInput input)
        {

            var filteredProductByVariants = _productByVariantRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ProductVariantFk)
                        .Include(e => e.ProductVariantCategoryFk)
                        .Include(e => e.MediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductVariantNameFilter), e => e.ProductVariantFk != null && e.ProductVariantFk.Name == input.ProductVariantNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductVariantCategoryNameFilter), e => e.ProductVariantCategoryFk != null && e.ProductVariantCategoryFk.Name == input.ProductVariantCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.MediaLibraryFk != null && e.MediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var pagedAndFilteredProductByVariants = filteredProductByVariants
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productByVariants = from o in pagedAndFilteredProductByVariants
                                    join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _lookup_productVariantRepository.GetAll() on o.ProductVariantId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()

                                    join o3 in _lookup_productVariantCategoryRepository.GetAll() on o.ProductVariantCategoryId equals o3.Id into j3
                                    from s3 in j3.DefaultIfEmpty()

                                    join o4 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o4.Id into j4
                                    from s4 in j4.DefaultIfEmpty()

                                    select new
                                    {

                                        o.Price,
                                        o.DisplaySequence,
                                        o.Description,
                                        Id = o.Id,
                                        ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                        ProductVariantName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                        ProductVariantCategoryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                        MediaLibraryName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                    };

            var totalCount = await filteredProductByVariants.CountAsync();

            var dbList = await productByVariants.ToListAsync();
            var results = new List<GetProductByVariantForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductByVariantForViewDto()
                {
                    ProductByVariant = new ProductByVariantDto
                    {

                        Price = o.Price,
                        DisplaySequence = o.DisplaySequence,
                        Description = o.Description,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    ProductVariantName = o.ProductVariantName,
                    ProductVariantCategoryName = o.ProductVariantCategoryName,
                    MediaLibraryName = o.MediaLibraryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductByVariantForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductByVariantForViewDto> GetProductByVariantForView(long id)
        {
            var productByVariant = await _productByVariantRepository.GetAsync(id);

            var output = new GetProductByVariantForViewDto { ProductByVariant = ObjectMapper.Map<ProductByVariantDto>(productByVariant) };

            if (output.ProductByVariant.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductByVariant.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductByVariant.ProductVariantId != null)
            {
                var _lookupProductVariant = await _lookup_productVariantRepository.FirstOrDefaultAsync((long)output.ProductByVariant.ProductVariantId);
                output.ProductVariantName = _lookupProductVariant?.Name?.ToString();
            }

            if (output.ProductByVariant.ProductVariantCategoryId != null)
            {
                var _lookupProductVariantCategory = await _lookup_productVariantCategoryRepository.FirstOrDefaultAsync((long)output.ProductByVariant.ProductVariantCategoryId);
                output.ProductVariantCategoryName = _lookupProductVariantCategory?.Name?.ToString();
            }

            if (output.ProductByVariant.MediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.ProductByVariant.MediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductByVariants_Edit)]
        public async Task<GetProductByVariantForEditOutput> GetProductByVariantForEdit(EntityDto<long> input)
        {
            var productByVariant = await _productByVariantRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductByVariantForEditOutput { ProductByVariant = ObjectMapper.Map<CreateOrEditProductByVariantDto>(productByVariant) };

            if (output.ProductByVariant.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductByVariant.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductByVariant.ProductVariantId != null)
            {
                var _lookupProductVariant = await _lookup_productVariantRepository.FirstOrDefaultAsync((long)output.ProductByVariant.ProductVariantId);
                output.ProductVariantName = _lookupProductVariant?.Name?.ToString();
            }

            if (output.ProductByVariant.ProductVariantCategoryId != null)
            {
                var _lookupProductVariantCategory = await _lookup_productVariantCategoryRepository.FirstOrDefaultAsync((long)output.ProductByVariant.ProductVariantCategoryId);
                output.ProductVariantCategoryName = _lookupProductVariantCategory?.Name?.ToString();
            }

            if (output.ProductByVariant.MediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.ProductByVariant.MediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductByVariantDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductByVariants_Create)]
        protected virtual async Task Create(CreateOrEditProductByVariantDto input)
        {
            var productByVariant = ObjectMapper.Map<ProductByVariant>(input);

            if (AbpSession.TenantId != null)
            {
                productByVariant.TenantId = (int?)AbpSession.TenantId;
            }

            await _productByVariantRepository.InsertAsync(productByVariant);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductByVariants_Edit)]
        protected virtual async Task Update(CreateOrEditProductByVariantDto input)
        {
            var productByVariant = await _productByVariantRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productByVariant);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductByVariants_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productByVariantRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductByVariantsToExcel(GetAllProductByVariantsForExcelInput input)
        {

            var filteredProductByVariants = _productByVariantRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ProductVariantFk)
                        .Include(e => e.ProductVariantCategoryFk)
                        .Include(e => e.MediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductVariantNameFilter), e => e.ProductVariantFk != null && e.ProductVariantFk.Name == input.ProductVariantNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductVariantCategoryNameFilter), e => e.ProductVariantCategoryFk != null && e.ProductVariantCategoryFk.Name == input.ProductVariantCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.MediaLibraryFk != null && e.MediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var query = (from o in filteredProductByVariants
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productVariantRepository.GetAll() on o.ProductVariantId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_productVariantCategoryRepository.GetAll() on o.ProductVariantCategoryId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetProductByVariantForViewDto()
                         {
                             ProductByVariant = new ProductByVariantDto
                             {
                                 Price = o.Price,
                                 DisplaySequence = o.DisplaySequence,
                                 Description = o.Description,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductVariantName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ProductVariantCategoryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             MediaLibraryName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var productByVariantListDtos = await query.ToListAsync();

            return _productByVariantsExcelExporter.ExportToFile(productByVariantListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductByVariants)]
        public async Task<PagedResultDto<ProductByVariantProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductByVariantProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductByVariantProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductByVariantProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductByVariants)]
        public async Task<PagedResultDto<ProductByVariantProductVariantLookupTableDto>> GetAllProductVariantForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productVariantRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productVariantList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductByVariantProductVariantLookupTableDto>();
            foreach (var productVariant in productVariantList)
            {
                lookupTableDtoList.Add(new ProductByVariantProductVariantLookupTableDto
                {
                    Id = productVariant.Id,
                    DisplayName = productVariant.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductByVariantProductVariantLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductByVariants)]
        public async Task<PagedResultDto<ProductByVariantProductVariantCategoryLookupTableDto>> GetAllProductVariantCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productVariantCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productVariantCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductByVariantProductVariantCategoryLookupTableDto>();
            foreach (var productVariantCategory in productVariantCategoryList)
            {
                lookupTableDtoList.Add(new ProductByVariantProductVariantCategoryLookupTableDto
                {
                    Id = productVariantCategory.Id,
                    DisplayName = productVariantCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductByVariantProductVariantCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductByVariants)]
        public async Task<PagedResultDto<ProductByVariantMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_mediaLibraryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var mediaLibraryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductByVariantMediaLibraryLookupTableDto>();
            foreach (var mediaLibrary in mediaLibraryList)
            {
                lookupTableDtoList.Add(new ProductByVariantMediaLibraryLookupTableDto
                {
                    Id = mediaLibrary.Id,
                    DisplayName = mediaLibrary.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductByVariantMediaLibraryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}