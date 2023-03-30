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
    [AbpAuthorize(AppPermissions.Pages_ProductMedias)]
    public class ProductMediasAppService : SoftGridAppServiceBase, IProductMediasAppService
    {
        private readonly IRepository<ProductMedia, long> _productMediaRepository;
        private readonly IProductMediasExcelExporter _productMediasExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<MediaLibrary, long> _lookup_mediaLibraryRepository;

        public ProductMediasAppService(IRepository<ProductMedia, long> productMediaRepository, IProductMediasExcelExporter productMediasExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<MediaLibrary, long> lookup_mediaLibraryRepository)
        {
            _productMediaRepository = productMediaRepository;
            _productMediasExcelExporter = productMediasExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_mediaLibraryRepository = lookup_mediaLibraryRepository;

        }

        public async Task<PagedResultDto<GetProductMediaForViewDto>> GetAll(GetAllProductMediasInput input)
        {

            var filteredProductMedias = _productMediaRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.MediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.MediaLibraryFk != null && e.MediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var pagedAndFilteredProductMedias = filteredProductMedias
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productMedias = from o in pagedAndFilteredProductMedias
                                join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                select new
                                {

                                    o.DisplaySequence,
                                    Id = o.Id,
                                    ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                    MediaLibraryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                };

            var totalCount = await filteredProductMedias.CountAsync();

            var dbList = await productMedias.ToListAsync();
            var results = new List<GetProductMediaForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductMediaForViewDto()
                {
                    ProductMedia = new ProductMediaDto
                    {

                        DisplaySequence = o.DisplaySequence,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    MediaLibraryName = o.MediaLibraryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductMediaForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductMediaForViewDto> GetProductMediaForView(long id)
        {
            var productMedia = await _productMediaRepository.GetAsync(id);

            var output = new GetProductMediaForViewDto { ProductMedia = ObjectMapper.Map<ProductMediaDto>(productMedia) };

            if (output.ProductMedia.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductMedia.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductMedia.MediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.ProductMedia.MediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductMedias_Edit)]
        public async Task<GetProductMediaForEditOutput> GetProductMediaForEdit(EntityDto<long> input)
        {
            var productMedia = await _productMediaRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductMediaForEditOutput { ProductMedia = ObjectMapper.Map<CreateOrEditProductMediaDto>(productMedia) };

            if (output.ProductMedia.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductMedia.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductMedia.MediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.ProductMedia.MediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductMediaDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductMedias_Create)]
        protected virtual async Task Create(CreateOrEditProductMediaDto input)
        {
            var productMedia = ObjectMapper.Map<ProductMedia>(input);

            if (AbpSession.TenantId != null)
            {
                productMedia.TenantId = (int?)AbpSession.TenantId;
            }

            await _productMediaRepository.InsertAsync(productMedia);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductMedias_Edit)]
        protected virtual async Task Update(CreateOrEditProductMediaDto input)
        {
            var productMedia = await _productMediaRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productMedia);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductMedias_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productMediaRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductMediasToExcel(GetAllProductMediasForExcelInput input)
        {

            var filteredProductMedias = _productMediaRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.MediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.MediaLibraryFk != null && e.MediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var query = (from o in filteredProductMedias
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProductMediaForViewDto()
                         {
                             ProductMedia = new ProductMediaDto
                             {
                                 DisplaySequence = o.DisplaySequence,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MediaLibraryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var productMediaListDtos = await query.ToListAsync();

            return _productMediasExcelExporter.ExportToFile(productMediaListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductMedias)]
        public async Task<PagedResultDto<ProductMediaProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductMediaProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductMediaProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductMediaProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductMedias)]
        public async Task<PagedResultDto<ProductMediaMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_mediaLibraryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var mediaLibraryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductMediaMediaLibraryLookupTableDto>();
            foreach (var mediaLibrary in mediaLibraryList)
            {
                lookupTableDtoList.Add(new ProductMediaMediaLibraryLookupTableDto
                {
                    Id = mediaLibrary.Id,
                    DisplayName = mediaLibrary.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductMediaMediaLibraryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}