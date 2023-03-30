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
    [AbpAuthorize(AppPermissions.Pages_ProductCategoryMaps)]
    public class ProductCategoryMapsAppService : SoftGridAppServiceBase, IProductCategoryMapsAppService
    {
        private readonly IRepository<ProductCategoryMap, long> _productCategoryMapRepository;
        private readonly IProductCategoryMapsExcelExporter _productCategoryMapsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<ProductCategory, long> _lookup_productCategoryRepository;

        public ProductCategoryMapsAppService(IRepository<ProductCategoryMap, long> productCategoryMapRepository, IProductCategoryMapsExcelExporter productCategoryMapsExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<ProductCategory, long> lookup_productCategoryRepository)
        {
            _productCategoryMapRepository = productCategoryMapRepository;
            _productCategoryMapsExcelExporter = productCategoryMapsExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;

        }

        public async Task<PagedResultDto<GetProductCategoryMapForViewDto>> GetAll(GetAllProductCategoryMapsInput input)
        {

            var filteredProductCategoryMaps = _productCategoryMapRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter);

            var pagedAndFilteredProductCategoryMaps = filteredProductCategoryMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productCategoryMaps = from o in pagedAndFilteredProductCategoryMaps
                                      join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                      from s1 in j1.DefaultIfEmpty()

                                      join o2 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o2.Id into j2
                                      from s2 in j2.DefaultIfEmpty()

                                      select new
                                      {

                                          Id = o.Id,
                                          ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                          ProductCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                      };

            var totalCount = await filteredProductCategoryMaps.CountAsync();

            var dbList = await productCategoryMaps.ToListAsync();
            var results = new List<GetProductCategoryMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductCategoryMapForViewDto()
                {
                    ProductCategoryMap = new ProductCategoryMapDto
                    {

                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    ProductCategoryName = o.ProductCategoryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductCategoryMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductCategoryMapForViewDto> GetProductCategoryMapForView(long id)
        {
            var productCategoryMap = await _productCategoryMapRepository.GetAsync(id);

            var output = new GetProductCategoryMapForViewDto { ProductCategoryMap = ObjectMapper.Map<ProductCategoryMapDto>(productCategoryMap) };

            if (output.ProductCategoryMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductCategoryMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductCategoryMap.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.ProductCategoryMap.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryMaps_Edit)]
        public async Task<GetProductCategoryMapForEditOutput> GetProductCategoryMapForEdit(EntityDto<long> input)
        {
            var productCategoryMap = await _productCategoryMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductCategoryMapForEditOutput { ProductCategoryMap = ObjectMapper.Map<CreateOrEditProductCategoryMapDto>(productCategoryMap) };

            if (output.ProductCategoryMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductCategoryMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductCategoryMap.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.ProductCategoryMap.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductCategoryMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryMaps_Create)]
        protected virtual async Task Create(CreateOrEditProductCategoryMapDto input)
        {
            var productCategoryMap = ObjectMapper.Map<ProductCategoryMap>(input);

            if (AbpSession.TenantId != null)
            {
                productCategoryMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _productCategoryMapRepository.InsertAsync(productCategoryMap);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryMaps_Edit)]
        protected virtual async Task Update(CreateOrEditProductCategoryMapDto input)
        {
            var productCategoryMap = await _productCategoryMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productCategoryMap);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productCategoryMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductCategoryMapsToExcel(GetAllProductCategoryMapsForExcelInput input)
        {

            var filteredProductCategoryMaps = _productCategoryMapRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter);

            var query = (from o in filteredProductCategoryMaps
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProductCategoryMapForViewDto()
                         {
                             ProductCategoryMap = new ProductCategoryMapDto
                             {
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var productCategoryMapListDtos = await query.ToListAsync();

            return _productCategoryMapsExcelExporter.ExportToFile(productCategoryMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryMaps)]
        public async Task<PagedResultDto<ProductCategoryMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCategoryMapProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductCategoryMapProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductCategoryMapProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryMaps)]
        public async Task<PagedResultDto<ProductCategoryMapProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCategoryMapProductCategoryLookupTableDto>();
            foreach (var productCategory in productCategoryList)
            {
                lookupTableDtoList.Add(new ProductCategoryMapProductCategoryLookupTableDto
                {
                    Id = productCategory.Id,
                    DisplayName = productCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductCategoryMapProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}