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
    [AbpAuthorize(AppPermissions.Pages_ProductVariants)]
    public class ProductVariantsAppService : SoftGridAppServiceBase, IProductVariantsAppService
    {
        private readonly IRepository<ProductVariant, long> _productVariantRepository;
        private readonly IProductVariantsExcelExporter _productVariantsExcelExporter;
        private readonly IRepository<ProductVariantCategory, long> _lookup_productVariantCategoryRepository;

        public ProductVariantsAppService(IRepository<ProductVariant, long> productVariantRepository, IProductVariantsExcelExporter productVariantsExcelExporter, IRepository<ProductVariantCategory, long> lookup_productVariantCategoryRepository)
        {
            _productVariantRepository = productVariantRepository;
            _productVariantsExcelExporter = productVariantsExcelExporter;
            _lookup_productVariantCategoryRepository = lookup_productVariantCategoryRepository;

        }

        public async Task<PagedResultDto<GetProductVariantForViewDto>> GetAll(GetAllProductVariantsInput input)
        {

            var filteredProductVariants = _productVariantRepository.GetAll()
                        .Include(e => e.ProductVariantCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductVariantCategoryNameFilter), e => e.ProductVariantCategoryFk != null && e.ProductVariantCategoryFk.Name == input.ProductVariantCategoryNameFilter);

            var pagedAndFilteredProductVariants = filteredProductVariants
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productVariants = from o in pagedAndFilteredProductVariants
                                  join o1 in _lookup_productVariantCategoryRepository.GetAll() on o.ProductVariantCategoryId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  select new
                                  {

                                      o.Name,
                                      Id = o.Id,
                                      ProductVariantCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                  };

            var totalCount = await filteredProductVariants.CountAsync();

            var dbList = await productVariants.ToListAsync();
            var results = new List<GetProductVariantForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductVariantForViewDto()
                {
                    ProductVariant = new ProductVariantDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    },
                    ProductVariantCategoryName = o.ProductVariantCategoryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductVariantForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductVariantForViewDto> GetProductVariantForView(long id)
        {
            var productVariant = await _productVariantRepository.GetAsync(id);

            var output = new GetProductVariantForViewDto { ProductVariant = ObjectMapper.Map<ProductVariantDto>(productVariant) };

            if (output.ProductVariant.ProductVariantCategoryId != null)
            {
                var _lookupProductVariantCategory = await _lookup_productVariantCategoryRepository.FirstOrDefaultAsync((long)output.ProductVariant.ProductVariantCategoryId);
                output.ProductVariantCategoryName = _lookupProductVariantCategory?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductVariants_Edit)]
        public async Task<GetProductVariantForEditOutput> GetProductVariantForEdit(EntityDto<long> input)
        {
            var productVariant = await _productVariantRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductVariantForEditOutput { ProductVariant = ObjectMapper.Map<CreateOrEditProductVariantDto>(productVariant) };

            if (output.ProductVariant.ProductVariantCategoryId != null)
            {
                var _lookupProductVariantCategory = await _lookup_productVariantCategoryRepository.FirstOrDefaultAsync((long)output.ProductVariant.ProductVariantCategoryId);
                output.ProductVariantCategoryName = _lookupProductVariantCategory?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductVariantDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductVariants_Create)]
        protected virtual async Task Create(CreateOrEditProductVariantDto input)
        {
            var productVariant = ObjectMapper.Map<ProductVariant>(input);

            if (AbpSession.TenantId != null)
            {
                productVariant.TenantId = (int?)AbpSession.TenantId;
            }

            await _productVariantRepository.InsertAsync(productVariant);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductVariants_Edit)]
        protected virtual async Task Update(CreateOrEditProductVariantDto input)
        {
            var productVariant = await _productVariantRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productVariant);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductVariants_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productVariantRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductVariantsToExcel(GetAllProductVariantsForExcelInput input)
        {

            var filteredProductVariants = _productVariantRepository.GetAll()
                        .Include(e => e.ProductVariantCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductVariantCategoryNameFilter), e => e.ProductVariantCategoryFk != null && e.ProductVariantCategoryFk.Name == input.ProductVariantCategoryNameFilter);

            var query = (from o in filteredProductVariants
                         join o1 in _lookup_productVariantCategoryRepository.GetAll() on o.ProductVariantCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetProductVariantForViewDto()
                         {
                             ProductVariant = new ProductVariantDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             },
                             ProductVariantCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var productVariantListDtos = await query.ToListAsync();

            return _productVariantsExcelExporter.ExportToFile(productVariantListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductVariants)]
        public async Task<PagedResultDto<ProductVariantProductVariantCategoryLookupTableDto>> GetAllProductVariantCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productVariantCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productVariantCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductVariantProductVariantCategoryLookupTableDto>();
            foreach (var productVariantCategory in productVariantCategoryList)
            {
                lookupTableDtoList.Add(new ProductVariantProductVariantCategoryLookupTableDto
                {
                    Id = productVariantCategory.Id,
                    DisplayName = productVariantCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductVariantProductVariantCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}