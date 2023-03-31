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
    [AbpAuthorize(AppPermissions.Pages_ProductCrossSellProducts)]
    public class ProductCrossSellProductsAppService : SoftGridAppServiceBase, IProductCrossSellProductsAppService
    {
        private readonly IRepository<ProductCrossSellProduct, long> _productCrossSellProductRepository;
        private readonly IProductCrossSellProductsExcelExporter _productCrossSellProductsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;

        public ProductCrossSellProductsAppService(IRepository<ProductCrossSellProduct, long> productCrossSellProductRepository, IProductCrossSellProductsExcelExporter productCrossSellProductsExcelExporter, IRepository<Product, long> lookup_productRepository)
        {
            _productCrossSellProductRepository = productCrossSellProductRepository;
            _productCrossSellProductsExcelExporter = productCrossSellProductsExcelExporter;
            _lookup_productRepository = lookup_productRepository;

        }

        public async Task<PagedResultDto<GetProductCrossSellProductForViewDto>> GetAll(GetAllProductCrossSellProductsInput input)
        {

            var filteredProductCrossSellProducts = _productCrossSellProductRepository.GetAll()
                        .Include(e => e.PrimaryProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinCrossProductIdFilter != null, e => e.CrossProductId >= input.MinCrossProductIdFilter)
                        .WhereIf(input.MaxCrossProductIdFilter != null, e => e.CrossProductId <= input.MaxCrossProductIdFilter)
                        .WhereIf(input.MinCrossSellScoreFilter != null, e => e.CrossSellScore >= input.MinCrossSellScoreFilter)
                        .WhereIf(input.MaxCrossSellScoreFilter != null, e => e.CrossSellScore <= input.MaxCrossSellScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.PrimaryProductFk != null && e.PrimaryProductFk.Name == input.ProductNameFilter);

            var pagedAndFilteredProductCrossSellProducts = filteredProductCrossSellProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productCrossSellProducts = from o in pagedAndFilteredProductCrossSellProducts
                                           join o1 in _lookup_productRepository.GetAll() on o.PrimaryProductId equals o1.Id into j1
                                           from s1 in j1.DefaultIfEmpty()

                                           select new
                                           {

                                               o.CrossProductId,
                                               o.CrossSellScore,
                                               Id = o.Id,
                                               ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                           };

            var totalCount = await filteredProductCrossSellProducts.CountAsync();

            var dbList = await productCrossSellProducts.ToListAsync();
            var results = new List<GetProductCrossSellProductForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductCrossSellProductForViewDto()
                {
                    ProductCrossSellProduct = new ProductCrossSellProductDto
                    {

                        CrossProductId = o.CrossProductId,
                        CrossSellScore = o.CrossSellScore,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductCrossSellProductForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductCrossSellProductForViewDto> GetProductCrossSellProductForView(long id)
        {
            var productCrossSellProduct = await _productCrossSellProductRepository.GetAsync(id);

            var output = new GetProductCrossSellProductForViewDto { ProductCrossSellProduct = ObjectMapper.Map<ProductCrossSellProductDto>(productCrossSellProduct) };

            if (output.ProductCrossSellProduct.PrimaryProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductCrossSellProduct.PrimaryProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCrossSellProducts_Edit)]
        public async Task<GetProductCrossSellProductForEditOutput> GetProductCrossSellProductForEdit(EntityDto<long> input)
        {
            var productCrossSellProduct = await _productCrossSellProductRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductCrossSellProductForEditOutput { ProductCrossSellProduct = ObjectMapper.Map<CreateOrEditProductCrossSellProductDto>(productCrossSellProduct) };

            if (output.ProductCrossSellProduct.PrimaryProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductCrossSellProduct.PrimaryProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductCrossSellProductDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductCrossSellProducts_Create)]
        protected virtual async Task Create(CreateOrEditProductCrossSellProductDto input)
        {
            var productCrossSellProduct = ObjectMapper.Map<ProductCrossSellProduct>(input);

            if (AbpSession.TenantId != null)
            {
                productCrossSellProduct.TenantId = (int?)AbpSession.TenantId;
            }

            await _productCrossSellProductRepository.InsertAsync(productCrossSellProduct);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductCrossSellProducts_Edit)]
        protected virtual async Task Update(CreateOrEditProductCrossSellProductDto input)
        {
            var productCrossSellProduct = await _productCrossSellProductRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productCrossSellProduct);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductCrossSellProducts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productCrossSellProductRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductCrossSellProductsToExcel(GetAllProductCrossSellProductsForExcelInput input)
        {

            var filteredProductCrossSellProducts = _productCrossSellProductRepository.GetAll()
                        .Include(e => e.PrimaryProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinCrossProductIdFilter != null, e => e.CrossProductId >= input.MinCrossProductIdFilter)
                        .WhereIf(input.MaxCrossProductIdFilter != null, e => e.CrossProductId <= input.MaxCrossProductIdFilter)
                        .WhereIf(input.MinCrossSellScoreFilter != null, e => e.CrossSellScore >= input.MinCrossSellScoreFilter)
                        .WhereIf(input.MaxCrossSellScoreFilter != null, e => e.CrossSellScore <= input.MaxCrossSellScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.PrimaryProductFk != null && e.PrimaryProductFk.Name == input.ProductNameFilter);

            var query = (from o in filteredProductCrossSellProducts
                         join o1 in _lookup_productRepository.GetAll() on o.PrimaryProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetProductCrossSellProductForViewDto()
                         {
                             ProductCrossSellProduct = new ProductCrossSellProductDto
                             {
                                 CrossProductId = o.CrossProductId,
                                 CrossSellScore = o.CrossSellScore,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var productCrossSellProductListDtos = await query.ToListAsync();

            return _productCrossSellProductsExcelExporter.ExportToFile(productCrossSellProductListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCrossSellProducts)]
        public async Task<PagedResultDto<ProductCrossSellProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCrossSellProductProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductCrossSellProductProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductCrossSellProductProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}