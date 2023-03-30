using SoftGrid.CRM;
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
    [AbpAuthorize(AppPermissions.Pages_ProductReviews)]
    public class ProductReviewsAppService : SoftGridAppServiceBase, IProductReviewsAppService
    {
        private readonly IRepository<ProductReview, long> _productReviewRepository;
        private readonly IProductReviewsExcelExporter _productReviewsExcelExporter;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<RatingLike, long> _lookup_ratingLikeRepository;

        public ProductReviewsAppService(IRepository<ProductReview, long> productReviewRepository, IProductReviewsExcelExporter productReviewsExcelExporter, IRepository<Contact, long> lookup_contactRepository, IRepository<Product, long> lookup_productRepository, IRepository<Store, long> lookup_storeRepository, IRepository<RatingLike, long> lookup_ratingLikeRepository)
        {
            _productReviewRepository = productReviewRepository;
            _productReviewsExcelExporter = productReviewsExcelExporter;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_ratingLikeRepository = lookup_ratingLikeRepository;

        }

        public async Task<PagedResultDto<GetProductReviewForViewDto>> GetAll(GetAllProductReviewsInput input)
        {

            var filteredProductReviews = _productReviewRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.ProductFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.RatingLikeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReviewInfo.Contains(input.Filter) || e.PostTime.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReviewInfoFilter), e => e.ReviewInfo.Contains(input.ReviewInfoFilter))
                        .WhereIf(input.MinPostDateFilter != null, e => e.PostDate >= input.MinPostDateFilter)
                        .WhereIf(input.MaxPostDateFilter != null, e => e.PostDate <= input.MaxPostDateFilter)
                        .WhereIf(input.PublishFilter.HasValue && input.PublishFilter > -1, e => (input.PublishFilter == 1 && e.Publish) || (input.PublishFilter == 0 && !e.Publish))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostTimeFilter), e => e.PostTime.Contains(input.PostTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RatingLikeNameFilter), e => e.RatingLikeFk != null && e.RatingLikeFk.Name == input.RatingLikeNameFilter);

            var pagedAndFilteredProductReviews = filteredProductReviews
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productReviews = from o in pagedAndFilteredProductReviews
                                 join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()

                                 join o3 in _lookup_storeRepository.GetAll() on o.StoreId equals o3.Id into j3
                                 from s3 in j3.DefaultIfEmpty()

                                 join o4 in _lookup_ratingLikeRepository.GetAll() on o.RatingLikeId equals o4.Id into j4
                                 from s4 in j4.DefaultIfEmpty()

                                 select new
                                 {

                                     o.ReviewInfo,
                                     o.PostDate,
                                     o.Publish,
                                     o.PostTime,
                                     Id = o.Id,
                                     ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                                     ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                     StoreName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                     RatingLikeName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                 };

            var totalCount = await filteredProductReviews.CountAsync();

            var dbList = await productReviews.ToListAsync();
            var results = new List<GetProductReviewForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductReviewForViewDto()
                {
                    ProductReview = new ProductReviewDto
                    {

                        ReviewInfo = o.ReviewInfo,
                        PostDate = o.PostDate,
                        Publish = o.Publish,
                        PostTime = o.PostTime,
                        Id = o.Id,
                    },
                    ContactFullName = o.ContactFullName,
                    ProductName = o.ProductName,
                    StoreName = o.StoreName,
                    RatingLikeName = o.RatingLikeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductReviewForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductReviewForViewDto> GetProductReviewForView(long id)
        {
            var productReview = await _productReviewRepository.GetAsync(id);

            var output = new GetProductReviewForViewDto { ProductReview = ObjectMapper.Map<ProductReviewDto>(productReview) };

            if (output.ProductReview.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ProductReview.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ProductReview.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductReview.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductReview.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.ProductReview.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.ProductReview.RatingLikeId != null)
            {
                var _lookupRatingLike = await _lookup_ratingLikeRepository.FirstOrDefaultAsync((long)output.ProductReview.RatingLikeId);
                output.RatingLikeName = _lookupRatingLike?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductReviews_Edit)]
        public async Task<GetProductReviewForEditOutput> GetProductReviewForEdit(EntityDto<long> input)
        {
            var productReview = await _productReviewRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductReviewForEditOutput { ProductReview = ObjectMapper.Map<CreateOrEditProductReviewDto>(productReview) };

            if (output.ProductReview.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ProductReview.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ProductReview.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductReview.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductReview.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.ProductReview.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.ProductReview.RatingLikeId != null)
            {
                var _lookupRatingLike = await _lookup_ratingLikeRepository.FirstOrDefaultAsync((long)output.ProductReview.RatingLikeId);
                output.RatingLikeName = _lookupRatingLike?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductReviewDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductReviews_Create)]
        protected virtual async Task Create(CreateOrEditProductReviewDto input)
        {
            var productReview = ObjectMapper.Map<ProductReview>(input);

            if (AbpSession.TenantId != null)
            {
                productReview.TenantId = (int?)AbpSession.TenantId;
            }

            await _productReviewRepository.InsertAsync(productReview);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductReviews_Edit)]
        protected virtual async Task Update(CreateOrEditProductReviewDto input)
        {
            var productReview = await _productReviewRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productReview);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductReviews_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productReviewRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductReviewsToExcel(GetAllProductReviewsForExcelInput input)
        {

            var filteredProductReviews = _productReviewRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.ProductFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.RatingLikeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReviewInfo.Contains(input.Filter) || e.PostTime.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReviewInfoFilter), e => e.ReviewInfo.Contains(input.ReviewInfoFilter))
                        .WhereIf(input.MinPostDateFilter != null, e => e.PostDate >= input.MinPostDateFilter)
                        .WhereIf(input.MaxPostDateFilter != null, e => e.PostDate <= input.MaxPostDateFilter)
                        .WhereIf(input.PublishFilter.HasValue && input.PublishFilter > -1, e => (input.PublishFilter == 1 && e.Publish) || (input.PublishFilter == 0 && !e.Publish))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostTimeFilter), e => e.PostTime.Contains(input.PostTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RatingLikeNameFilter), e => e.RatingLikeFk != null && e.RatingLikeFk.Name == input.RatingLikeNameFilter);

            var query = (from o in filteredProductReviews
                         join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_storeRepository.GetAll() on o.StoreId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_ratingLikeRepository.GetAll() on o.RatingLikeId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetProductReviewForViewDto()
                         {
                             ProductReview = new ProductReviewDto
                             {
                                 ReviewInfo = o.ReviewInfo,
                                 PostDate = o.PostDate,
                                 Publish = o.Publish,
                                 PostTime = o.PostTime,
                                 Id = o.Id
                             },
                             ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                             ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             StoreName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             RatingLikeName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var productReviewListDtos = await query.ToListAsync();

            return _productReviewsExcelExporter.ExportToFile(productReviewListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductReviews)]
        public async Task<PagedResultDto<ProductReviewContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductReviewContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new ProductReviewContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<ProductReviewContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductReviews)]
        public async Task<PagedResultDto<ProductReviewProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductReviewProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductReviewProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductReviewProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductReviews)]
        public async Task<PagedResultDto<ProductReviewStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductReviewStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new ProductReviewStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductReviewStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductReviews)]
        public async Task<PagedResultDto<ProductReviewRatingLikeLookupTableDto>> GetAllRatingLikeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_ratingLikeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var ratingLikeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductReviewRatingLikeLookupTableDto>();
            foreach (var ratingLike in ratingLikeList)
            {
                lookupTableDtoList.Add(new ProductReviewRatingLikeLookupTableDto
                {
                    Id = ratingLike.Id,
                    DisplayName = ratingLike.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductReviewRatingLikeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}