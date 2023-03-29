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
    [AbpAuthorize(AppPermissions.Pages_ProductVariantCategories)]
    public class ProductVariantCategoriesAppService : SoftGridAppServiceBase, IProductVariantCategoriesAppService
    {
        private readonly IRepository<ProductVariantCategory, long> _productVariantCategoryRepository;
        private readonly IProductVariantCategoriesExcelExporter _productVariantCategoriesExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;

        public ProductVariantCategoriesAppService(IRepository<ProductVariantCategory, long> productVariantCategoryRepository, IProductVariantCategoriesExcelExporter productVariantCategoriesExcelExporter, IRepository<Store, long> lookup_storeRepository)
        {
            _productVariantCategoryRepository = productVariantCategoryRepository;
            _productVariantCategoriesExcelExporter = productVariantCategoriesExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;

        }

        public async Task<PagedResultDto<GetProductVariantCategoryForViewDto>> GetAll(GetAllProductVariantCategoriesInput input)
        {

            var filteredProductVariantCategories = _productVariantCategoryRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var pagedAndFilteredProductVariantCategories = filteredProductVariantCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productVariantCategories = from o in pagedAndFilteredProductVariantCategories
                                           join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                                           from s1 in j1.DefaultIfEmpty()

                                           select new
                                           {

                                               o.Name,
                                               Id = o.Id,
                                               StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                           };

            var totalCount = await filteredProductVariantCategories.CountAsync();

            var dbList = await productVariantCategories.ToListAsync();
            var results = new List<GetProductVariantCategoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductVariantCategoryForViewDto()
                {
                    ProductVariantCategory = new ProductVariantCategoryDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductVariantCategoryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductVariantCategoryForViewDto> GetProductVariantCategoryForView(long id)
        {
            var productVariantCategory = await _productVariantCategoryRepository.GetAsync(id);

            var output = new GetProductVariantCategoryForViewDto { ProductVariantCategory = ObjectMapper.Map<ProductVariantCategoryDto>(productVariantCategory) };

            if (output.ProductVariantCategory.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.ProductVariantCategory.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductVariantCategories_Edit)]
        public async Task<GetProductVariantCategoryForEditOutput> GetProductVariantCategoryForEdit(EntityDto<long> input)
        {
            var productVariantCategory = await _productVariantCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductVariantCategoryForEditOutput { ProductVariantCategory = ObjectMapper.Map<CreateOrEditProductVariantCategoryDto>(productVariantCategory) };

            if (output.ProductVariantCategory.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.ProductVariantCategory.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductVariantCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductVariantCategories_Create)]
        protected virtual async Task Create(CreateOrEditProductVariantCategoryDto input)
        {
            var productVariantCategory = ObjectMapper.Map<ProductVariantCategory>(input);

            if (AbpSession.TenantId != null)
            {
                productVariantCategory.TenantId = (int?)AbpSession.TenantId;
            }

            await _productVariantCategoryRepository.InsertAsync(productVariantCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductVariantCategories_Edit)]
        protected virtual async Task Update(CreateOrEditProductVariantCategoryDto input)
        {
            var productVariantCategory = await _productVariantCategoryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productVariantCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductVariantCategories_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productVariantCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductVariantCategoriesToExcel(GetAllProductVariantCategoriesForExcelInput input)
        {

            var filteredProductVariantCategories = _productVariantCategoryRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var query = (from o in filteredProductVariantCategories
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetProductVariantCategoryForViewDto()
                         {
                             ProductVariantCategory = new ProductVariantCategoryDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var productVariantCategoryListDtos = await query.ToListAsync();

            return _productVariantCategoriesExcelExporter.ExportToFile(productVariantCategoryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductVariantCategories)]
        public async Task<PagedResultDto<ProductVariantCategoryStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductVariantCategoryStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new ProductVariantCategoryStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductVariantCategoryStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}