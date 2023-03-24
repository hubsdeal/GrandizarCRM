using SoftGrid.Shop;
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
    [AbpAuthorize(AppPermissions.Pages_ProductTags)]
    public class ProductTagsAppService : SoftGridAppServiceBase, IProductTagsAppService
    {
        private readonly IRepository<ProductTag, long> _productTagRepository;
        private readonly IProductTagsExcelExporter _productTagsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;
        private readonly IRepository<MasterTag, long> _lookup_masterTagRepository;

        public ProductTagsAppService(IRepository<ProductTag, long> productTagRepository, IProductTagsExcelExporter productTagsExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository, IRepository<MasterTag, long> lookup_masterTagRepository)
        {
            _productTagRepository = productTagRepository;
            _productTagsExcelExporter = productTagsExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;
            _lookup_masterTagRepository = lookup_masterTagRepository;

        }

        public async Task<PagedResultDto<GetProductTagForViewDto>> GetAll(GetAllProductTagsInput input)
        {

            var filteredProductTags = _productTagRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTag.Contains(input.Filter) || e.TagValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagFilter), e => e.CustomTag.Contains(input.CustomTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagValueFilter), e => e.TagValue.Contains(input.TagValueFilter))
                        .WhereIf(input.VerifiedFilter.HasValue && input.VerifiedFilter > -1, e => (input.VerifiedFilter == 1 && e.Verified) || (input.VerifiedFilter == 0 && !e.Verified))
                        .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                        .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var pagedAndFilteredProductTags = filteredProductTags
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productTags = from o in pagedAndFilteredProductTags
                              join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                              from s2 in j2.DefaultIfEmpty()

                              join o3 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o3.Id into j3
                              from s3 in j3.DefaultIfEmpty()

                              select new
                              {

                                  o.CustomTag,
                                  o.TagValue,
                                  o.Verified,
                                  o.Sequence,
                                  Id = o.Id,
                                  ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                  MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                  MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                              };

            var totalCount = await filteredProductTags.CountAsync();

            var dbList = await productTags.ToListAsync();
            var results = new List<GetProductTagForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductTagForViewDto()
                {
                    ProductTag = new ProductTagDto
                    {

                        CustomTag = o.CustomTag,
                        TagValue = o.TagValue,
                        Verified = o.Verified,
                        Sequence = o.Sequence,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    MasterTagCategoryName = o.MasterTagCategoryName,
                    MasterTagName = o.MasterTagName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductTagForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductTagForViewDto> GetProductTagForView(long id)
        {
            var productTag = await _productTagRepository.GetAsync(id);

            var output = new GetProductTagForViewDto { ProductTag = ObjectMapper.Map<ProductTagDto>(productTag) };

            if (output.ProductTag.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductTag.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.ProductTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.ProductTag.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.ProductTag.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductTags_Edit)]
        public async Task<GetProductTagForEditOutput> GetProductTagForEdit(EntityDto<long> input)
        {
            var productTag = await _productTagRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductTagForEditOutput { ProductTag = ObjectMapper.Map<CreateOrEditProductTagDto>(productTag) };

            if (output.ProductTag.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductTag.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.ProductTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.ProductTag.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.ProductTag.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductTagDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductTags_Create)]
        protected virtual async Task Create(CreateOrEditProductTagDto input)
        {
            var productTag = ObjectMapper.Map<ProductTag>(input);

            if (AbpSession.TenantId != null)
            {
                productTag.TenantId = (int?)AbpSession.TenantId;
            }

            await _productTagRepository.InsertAsync(productTag);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductTags_Edit)]
        protected virtual async Task Update(CreateOrEditProductTagDto input)
        {
            var productTag = await _productTagRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productTag);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductTags_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productTagRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductTagsToExcel(GetAllProductTagsForExcelInput input)
        {

            var filteredProductTags = _productTagRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTag.Contains(input.Filter) || e.TagValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagFilter), e => e.CustomTag.Contains(input.CustomTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagValueFilter), e => e.TagValue.Contains(input.TagValueFilter))
                        .WhereIf(input.VerifiedFilter.HasValue && input.VerifiedFilter > -1, e => (input.VerifiedFilter == 1 && e.Verified) || (input.VerifiedFilter == 0 && !e.Verified))
                        .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                        .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var query = (from o in filteredProductTags
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetProductTagForViewDto()
                         {
                             ProductTag = new ProductTagDto
                             {
                                 CustomTag = o.CustomTag,
                                 TagValue = o.TagValue,
                                 Verified = o.Verified,
                                 Sequence = o.Sequence,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var productTagListDtos = await query.ToListAsync();

            return _productTagsExcelExporter.ExportToFile(productTagListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductTags)]
        public async Task<PagedResultDto<ProductTagProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductTagProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductTagProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductTagProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductTags)]
        public async Task<PagedResultDto<ProductTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductTagMasterTagCategoryLookupTableDto>();
            foreach (var masterTagCategory in masterTagCategoryList)
            {
                lookupTableDtoList.Add(new ProductTagMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductTagMasterTagCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductTags)]
        public async Task<PagedResultDto<ProductTagMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductTagMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new ProductTagMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductTagMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}