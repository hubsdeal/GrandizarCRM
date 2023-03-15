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
    [AbpAuthorize(AppPermissions.Pages_ProductCategories)]
    public class ProductCategoriesAppService : SoftGridAppServiceBase, IProductCategoriesAppService
    {
        private readonly IRepository<ProductCategory, long> _productCategoryRepository;
        private readonly IProductCategoriesExcelExporter _productCategoriesExcelExporter;
        private readonly IRepository<MediaLibrary, long> _lookup_mediaLibraryRepository;

        public ProductCategoriesAppService(IRepository<ProductCategory, long> productCategoryRepository, IProductCategoriesExcelExporter productCategoriesExcelExporter, IRepository<MediaLibrary, long> lookup_mediaLibraryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _productCategoriesExcelExporter = productCategoriesExcelExporter;
            _lookup_mediaLibraryRepository = lookup_mediaLibraryRepository;

        }

        public async Task<PagedResultDto<GetProductCategoryForViewDto>> GetAll(GetAllProductCategoriesInput input)
        {

            var filteredProductCategories = _productCategoryRepository.GetAll()
                        .Include(e => e.MediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Url.Contains(input.Filter) || e.MetaTitle.Contains(input.Filter) || e.MetaKeywords.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.HasParentCategoryFilter.HasValue && input.HasParentCategoryFilter > -1, e => (input.HasParentCategoryFilter == 1 && e.HasParentCategory) || (input.HasParentCategoryFilter == 0 && !e.HasParentCategory))
                        .WhereIf(input.MinParentCategoryIdFilter != null, e => e.ParentCategoryId >= input.MinParentCategoryIdFilter)
                        .WhereIf(input.MaxParentCategoryIdFilter != null, e => e.ParentCategoryId <= input.MaxParentCategoryIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter), e => e.Url.Contains(input.UrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaTitleFilter), e => e.MetaTitle.Contains(input.MetaTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaKeywordsFilter), e => e.MetaKeywords.Contains(input.MetaKeywordsFilter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.ProductOrServiceFilter.HasValue && input.ProductOrServiceFilter > -1, e => (input.ProductOrServiceFilter == 1 && e.ProductOrService) || (input.ProductOrServiceFilter == 0 && !e.ProductOrService))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.MediaLibraryFk != null && e.MediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var pagedAndFilteredProductCategories = filteredProductCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productCategories = from o in pagedAndFilteredProductCategories
                                    join o1 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    select new
                                    {

                                        o.Name,
                                        o.Description,
                                        o.HasParentCategory,
                                        o.ParentCategoryId,
                                        o.Url,
                                        o.MetaTitle,
                                        o.MetaKeywords,
                                        o.DisplaySequence,
                                        o.ProductOrService,
                                        Id = o.Id,
                                        MediaLibraryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                    };

            var totalCount = await filteredProductCategories.CountAsync();

            var dbList = await productCategories.ToListAsync();
            var results = new List<GetProductCategoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductCategoryForViewDto()
                {
                    ProductCategory = new ProductCategoryDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        HasParentCategory = o.HasParentCategory,
                        ParentCategoryId = o.ParentCategoryId,
                        Url = o.Url,
                        MetaTitle = o.MetaTitle,
                        MetaKeywords = o.MetaKeywords,
                        DisplaySequence = o.DisplaySequence,
                        ProductOrService = o.ProductOrService,
                        Id = o.Id,
                    },
                    MediaLibraryName = o.MediaLibraryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductCategoryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductCategoryForViewDto> GetProductCategoryForView(long id)
        {
            var productCategory = await _productCategoryRepository.GetAsync(id);

            var output = new GetProductCategoryForViewDto { ProductCategory = ObjectMapper.Map<ProductCategoryDto>(productCategory) };

            if (output.ProductCategory.MediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.ProductCategory.MediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategories_Edit)]
        public async Task<GetProductCategoryForEditOutput> GetProductCategoryForEdit(EntityDto<long> input)
        {
            var productCategory = await _productCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductCategoryForEditOutput { ProductCategory = ObjectMapper.Map<CreateOrEditProductCategoryDto>(productCategory) };

            if (output.ProductCategory.MediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.ProductCategory.MediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductCategories_Create)]
        protected virtual async Task Create(CreateOrEditProductCategoryDto input)
        {
            var productCategory = ObjectMapper.Map<ProductCategory>(input);

            if (AbpSession.TenantId != null)
            {
                productCategory.TenantId = (int?)AbpSession.TenantId;
            }

            await _productCategoryRepository.InsertAsync(productCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategories_Edit)]
        protected virtual async Task Update(CreateOrEditProductCategoryDto input)
        {
            var productCategory = await _productCategoryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategories_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductCategoriesToExcel(GetAllProductCategoriesForExcelInput input)
        {

            var filteredProductCategories = _productCategoryRepository.GetAll()
                        .Include(e => e.MediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Url.Contains(input.Filter) || e.MetaTitle.Contains(input.Filter) || e.MetaKeywords.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.HasParentCategoryFilter.HasValue && input.HasParentCategoryFilter > -1, e => (input.HasParentCategoryFilter == 1 && e.HasParentCategory) || (input.HasParentCategoryFilter == 0 && !e.HasParentCategory))
                        .WhereIf(input.MinParentCategoryIdFilter != null, e => e.ParentCategoryId >= input.MinParentCategoryIdFilter)
                        .WhereIf(input.MaxParentCategoryIdFilter != null, e => e.ParentCategoryId <= input.MaxParentCategoryIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter), e => e.Url.Contains(input.UrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaTitleFilter), e => e.MetaTitle.Contains(input.MetaTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaKeywordsFilter), e => e.MetaKeywords.Contains(input.MetaKeywordsFilter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.ProductOrServiceFilter.HasValue && input.ProductOrServiceFilter > -1, e => (input.ProductOrServiceFilter == 1 && e.ProductOrService) || (input.ProductOrServiceFilter == 0 && !e.ProductOrService))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.MediaLibraryFk != null && e.MediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var query = (from o in filteredProductCategories
                         join o1 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetProductCategoryForViewDto()
                         {
                             ProductCategory = new ProductCategoryDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 HasParentCategory = o.HasParentCategory,
                                 ParentCategoryId = o.ParentCategoryId,
                                 Url = o.Url,
                                 MetaTitle = o.MetaTitle,
                                 MetaKeywords = o.MetaKeywords,
                                 DisplaySequence = o.DisplaySequence,
                                 ProductOrService = o.ProductOrService,
                                 Id = o.Id
                             },
                             MediaLibraryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var productCategoryListDtos = await query.ToListAsync();

            return _productCategoriesExcelExporter.ExportToFile(productCategoryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategories)]
        public async Task<PagedResultDto<ProductCategoryMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_mediaLibraryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var mediaLibraryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCategoryMediaLibraryLookupTableDto>();
            foreach (var mediaLibrary in mediaLibraryList)
            {
                lookupTableDtoList.Add(new ProductCategoryMediaLibraryLookupTableDto
                {
                    Id = mediaLibrary.Id,
                    DisplayName = mediaLibrary.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductCategoryMediaLibraryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}