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
    [AbpAuthorize(AppPermissions.Pages_ProductUpsellRelatedProducts)]
    public class ProductUpsellRelatedProductsAppService : SoftGridAppServiceBase, IProductUpsellRelatedProductsAppService
    {
        private readonly IRepository<ProductUpsellRelatedProduct, long> _productUpsellRelatedProductRepository;
        private readonly IProductUpsellRelatedProductsExcelExporter _productUpsellRelatedProductsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;

        public ProductUpsellRelatedProductsAppService(IRepository<ProductUpsellRelatedProduct, long> productUpsellRelatedProductRepository, IProductUpsellRelatedProductsExcelExporter productUpsellRelatedProductsExcelExporter, IRepository<Product, long> lookup_productRepository)
        {
            _productUpsellRelatedProductRepository = productUpsellRelatedProductRepository;
            _productUpsellRelatedProductsExcelExporter = productUpsellRelatedProductsExcelExporter;
            _lookup_productRepository = lookup_productRepository;

        }

        public async Task<PagedResultDto<GetProductUpsellRelatedProductForViewDto>> GetAll(GetAllProductUpsellRelatedProductsInput input)
        {

            var filteredProductUpsellRelatedProducts = _productUpsellRelatedProductRepository.GetAll()
                        .Include(e => e.PrimaryProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinRelatedProductIdFilter != null, e => e.RelatedProductId >= input.MinRelatedProductIdFilter)
                        .WhereIf(input.MaxRelatedProductIdFilter != null, e => e.RelatedProductId <= input.MaxRelatedProductIdFilter)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.PrimaryProductFk != null && e.PrimaryProductFk.Name == input.ProductNameFilter);

            var pagedAndFilteredProductUpsellRelatedProducts = filteredProductUpsellRelatedProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productUpsellRelatedProducts = from o in pagedAndFilteredProductUpsellRelatedProducts
                                               join o1 in _lookup_productRepository.GetAll() on o.PrimaryProductId equals o1.Id into j1
                                               from s1 in j1.DefaultIfEmpty()

                                               select new
                                               {

                                                   o.RelatedProductId,
                                                   o.DisplaySequence,
                                                   Id = o.Id,
                                                   ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                               };

            var totalCount = await filteredProductUpsellRelatedProducts.CountAsync();

            var dbList = await productUpsellRelatedProducts.ToListAsync();
            var results = new List<GetProductUpsellRelatedProductForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductUpsellRelatedProductForViewDto()
                {
                    ProductUpsellRelatedProduct = new ProductUpsellRelatedProductDto
                    {

                        RelatedProductId = o.RelatedProductId,
                        DisplaySequence = o.DisplaySequence,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductUpsellRelatedProductForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductUpsellRelatedProductForViewDto> GetProductUpsellRelatedProductForView(long id)
        {
            var productUpsellRelatedProduct = await _productUpsellRelatedProductRepository.GetAsync(id);

            var output = new GetProductUpsellRelatedProductForViewDto { ProductUpsellRelatedProduct = ObjectMapper.Map<ProductUpsellRelatedProductDto>(productUpsellRelatedProduct) };

            if (output.ProductUpsellRelatedProduct.PrimaryProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductUpsellRelatedProduct.PrimaryProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductUpsellRelatedProducts_Edit)]
        public async Task<GetProductUpsellRelatedProductForEditOutput> GetProductUpsellRelatedProductForEdit(EntityDto<long> input)
        {
            var productUpsellRelatedProduct = await _productUpsellRelatedProductRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductUpsellRelatedProductForEditOutput { ProductUpsellRelatedProduct = ObjectMapper.Map<CreateOrEditProductUpsellRelatedProductDto>(productUpsellRelatedProduct) };

            if (output.ProductUpsellRelatedProduct.PrimaryProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductUpsellRelatedProduct.PrimaryProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductUpsellRelatedProductDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductUpsellRelatedProducts_Create)]
        protected virtual async Task Create(CreateOrEditProductUpsellRelatedProductDto input)
        {
            var productUpsellRelatedProduct = ObjectMapper.Map<ProductUpsellRelatedProduct>(input);

            if (AbpSession.TenantId != null)
            {
                productUpsellRelatedProduct.TenantId = (int?)AbpSession.TenantId;
            }

            await _productUpsellRelatedProductRepository.InsertAsync(productUpsellRelatedProduct);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductUpsellRelatedProducts_Edit)]
        protected virtual async Task Update(CreateOrEditProductUpsellRelatedProductDto input)
        {
            var productUpsellRelatedProduct = await _productUpsellRelatedProductRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productUpsellRelatedProduct);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductUpsellRelatedProducts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productUpsellRelatedProductRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductUpsellRelatedProductsToExcel(GetAllProductUpsellRelatedProductsForExcelInput input)
        {

            var filteredProductUpsellRelatedProducts = _productUpsellRelatedProductRepository.GetAll()
                        .Include(e => e.PrimaryProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinRelatedProductIdFilter != null, e => e.RelatedProductId >= input.MinRelatedProductIdFilter)
                        .WhereIf(input.MaxRelatedProductIdFilter != null, e => e.RelatedProductId <= input.MaxRelatedProductIdFilter)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.PrimaryProductFk != null && e.PrimaryProductFk.Name == input.ProductNameFilter);

            var query = (from o in filteredProductUpsellRelatedProducts
                         join o1 in _lookup_productRepository.GetAll() on o.PrimaryProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetProductUpsellRelatedProductForViewDto()
                         {
                             ProductUpsellRelatedProduct = new ProductUpsellRelatedProductDto
                             {
                                 RelatedProductId = o.RelatedProductId,
                                 DisplaySequence = o.DisplaySequence,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var productUpsellRelatedProductListDtos = await query.ToListAsync();

            return _productUpsellRelatedProductsExcelExporter.ExportToFile(productUpsellRelatedProductListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductUpsellRelatedProducts)]
        public async Task<PagedResultDto<ProductUpsellRelatedProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductUpsellRelatedProductProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductUpsellRelatedProductProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductUpsellRelatedProductProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}