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
    [AbpAuthorize(AppPermissions.Pages_ProductCategoryVariantMaps)]
    public class ProductCategoryVariantMapsAppService : SoftGridAppServiceBase, IProductCategoryVariantMapsAppService
    {
        private readonly IRepository<ProductCategoryVariantMap, long> _productCategoryVariantMapRepository;
        private readonly IProductCategoryVariantMapsExcelExporter _productCategoryVariantMapsExcelExporter;
        private readonly IRepository<ProductCategory, long> _lookup_productCategoryRepository;
        private readonly IRepository<ProductVariantCategory, long> _lookup_productVariantCategoryRepository;

        public ProductCategoryVariantMapsAppService(IRepository<ProductCategoryVariantMap, long> productCategoryVariantMapRepository, IProductCategoryVariantMapsExcelExporter productCategoryVariantMapsExcelExporter, IRepository<ProductCategory, long> lookup_productCategoryRepository, IRepository<ProductVariantCategory, long> lookup_productVariantCategoryRepository)
        {
            _productCategoryVariantMapRepository = productCategoryVariantMapRepository;
            _productCategoryVariantMapsExcelExporter = productCategoryVariantMapsExcelExporter;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;
            _lookup_productVariantCategoryRepository = lookup_productVariantCategoryRepository;

        }

        public async Task<PagedResultDto<GetProductCategoryVariantMapForViewDto>> GetAll(GetAllProductCategoryVariantMapsInput input)
        {

            var filteredProductCategoryVariantMaps = _productCategoryVariantMapRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.ProductVariantCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductVariantCategoryNameFilter), e => e.ProductVariantCategoryFk != null && e.ProductVariantCategoryFk.Name == input.ProductVariantCategoryNameFilter);

            var pagedAndFilteredProductCategoryVariantMaps = filteredProductCategoryVariantMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productCategoryVariantMaps = from o in pagedAndFilteredProductCategoryVariantMaps
                                             join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                                             from s1 in j1.DefaultIfEmpty()

                                             join o2 in _lookup_productVariantCategoryRepository.GetAll() on o.ProductVariantCategoryId equals o2.Id into j2
                                             from s2 in j2.DefaultIfEmpty()

                                             select new
                                             {

                                                 Id = o.Id,
                                                 ProductCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                                 ProductVariantCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                             };

            var totalCount = await filteredProductCategoryVariantMaps.CountAsync();

            var dbList = await productCategoryVariantMaps.ToListAsync();
            var results = new List<GetProductCategoryVariantMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductCategoryVariantMapForViewDto()
                {
                    ProductCategoryVariantMap = new ProductCategoryVariantMapDto
                    {

                        Id = o.Id,
                    },
                    ProductCategoryName = o.ProductCategoryName,
                    ProductVariantCategoryName = o.ProductVariantCategoryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductCategoryVariantMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductCategoryVariantMapForViewDto> GetProductCategoryVariantMapForView(long id)
        {
            var productCategoryVariantMap = await _productCategoryVariantMapRepository.GetAsync(id);

            var output = new GetProductCategoryVariantMapForViewDto { ProductCategoryVariantMap = ObjectMapper.Map<ProductCategoryVariantMapDto>(productCategoryVariantMap) };

            if (output.ProductCategoryVariantMap.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.ProductCategoryVariantMap.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.ProductCategoryVariantMap.ProductVariantCategoryId != null)
            {
                var _lookupProductVariantCategory = await _lookup_productVariantCategoryRepository.FirstOrDefaultAsync((long)output.ProductCategoryVariantMap.ProductVariantCategoryId);
                output.ProductVariantCategoryName = _lookupProductVariantCategory?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryVariantMaps_Edit)]
        public async Task<GetProductCategoryVariantMapForEditOutput> GetProductCategoryVariantMapForEdit(EntityDto<long> input)
        {
            var productCategoryVariantMap = await _productCategoryVariantMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductCategoryVariantMapForEditOutput { ProductCategoryVariantMap = ObjectMapper.Map<CreateOrEditProductCategoryVariantMapDto>(productCategoryVariantMap) };

            if (output.ProductCategoryVariantMap.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.ProductCategoryVariantMap.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.ProductCategoryVariantMap.ProductVariantCategoryId != null)
            {
                var _lookupProductVariantCategory = await _lookup_productVariantCategoryRepository.FirstOrDefaultAsync((long)output.ProductCategoryVariantMap.ProductVariantCategoryId);
                output.ProductVariantCategoryName = _lookupProductVariantCategory?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductCategoryVariantMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryVariantMaps_Create)]
        protected virtual async Task Create(CreateOrEditProductCategoryVariantMapDto input)
        {
            var productCategoryVariantMap = ObjectMapper.Map<ProductCategoryVariantMap>(input);

            if (AbpSession.TenantId != null)
            {
                productCategoryVariantMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _productCategoryVariantMapRepository.InsertAsync(productCategoryVariantMap);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryVariantMaps_Edit)]
        protected virtual async Task Update(CreateOrEditProductCategoryVariantMapDto input)
        {
            var productCategoryVariantMap = await _productCategoryVariantMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productCategoryVariantMap);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryVariantMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productCategoryVariantMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductCategoryVariantMapsToExcel(GetAllProductCategoryVariantMapsForExcelInput input)
        {

            var filteredProductCategoryVariantMaps = _productCategoryVariantMapRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.ProductVariantCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductVariantCategoryNameFilter), e => e.ProductVariantCategoryFk != null && e.ProductVariantCategoryFk.Name == input.ProductVariantCategoryNameFilter);

            var query = (from o in filteredProductCategoryVariantMaps
                         join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productVariantCategoryRepository.GetAll() on o.ProductVariantCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProductCategoryVariantMapForViewDto()
                         {
                             ProductCategoryVariantMap = new ProductCategoryVariantMapDto
                             {
                                 Id = o.Id
                             },
                             ProductCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductVariantCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var productCategoryVariantMapListDtos = await query.ToListAsync();

            return _productCategoryVariantMapsExcelExporter.ExportToFile(productCategoryVariantMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryVariantMaps)]
        public async Task<PagedResultDto<ProductCategoryVariantMapProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCategoryVariantMapProductCategoryLookupTableDto>();
            foreach (var productCategory in productCategoryList)
            {
                lookupTableDtoList.Add(new ProductCategoryVariantMapProductCategoryLookupTableDto
                {
                    Id = productCategory.Id,
                    DisplayName = productCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductCategoryVariantMapProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryVariantMaps)]
        public async Task<PagedResultDto<ProductCategoryVariantMapProductVariantCategoryLookupTableDto>> GetAllProductVariantCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productVariantCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productVariantCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCategoryVariantMapProductVariantCategoryLookupTableDto>();
            foreach (var productVariantCategory in productVariantCategoryList)
            {
                lookupTableDtoList.Add(new ProductCategoryVariantMapProductVariantCategoryLookupTableDto
                {
                    Id = productVariantCategory.Id,
                    DisplayName = productVariantCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductCategoryVariantMapProductVariantCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}