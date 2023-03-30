using SoftGrid.Shop;
using SoftGrid.CRM;
using SoftGrid.Shop;
using SoftGrid.Territory;
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
    [AbpAuthorize(AppPermissions.Pages_ProductCustomerStats)]
    public class ProductCustomerStatsAppService : SoftGridAppServiceBase, IProductCustomerStatsAppService
    {
        private readonly IRepository<ProductCustomerStat, long> _productCustomerStatRepository;
        private readonly IProductCustomerStatsExcelExporter _productCustomerStatsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<Hub, long> _lookup_hubRepository;
        private readonly IRepository<SocialMedia, long> _lookup_socialMediaRepository;

        public ProductCustomerStatsAppService(IRepository<ProductCustomerStat, long> productCustomerStatRepository, IProductCustomerStatsExcelExporter productCustomerStatsExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<Contact, long> lookup_contactRepository, IRepository<Store, long> lookup_storeRepository, IRepository<Hub, long> lookup_hubRepository, IRepository<SocialMedia, long> lookup_socialMediaRepository)
        {
            _productCustomerStatRepository = productCustomerStatRepository;
            _productCustomerStatsExcelExporter = productCustomerStatsExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_hubRepository = lookup_hubRepository;
            _lookup_socialMediaRepository = lookup_socialMediaRepository;

        }

        public async Task<PagedResultDto<GetProductCustomerStatForViewDto>> GetAll(GetAllProductCustomerStatsInput input)
        {

            var filteredProductCustomerStats = _productCustomerStatRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.HubFk)
                        .Include(e => e.SocialMediaFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PageLink.Contains(input.Filter) || e.QuitFromLink.Contains(input.Filter))
                        .WhereIf(input.ClickedFilter.HasValue && input.ClickedFilter > -1, e => (input.ClickedFilter == 1 && e.Clicked) || (input.ClickedFilter == 0 && !e.Clicked))
                        .WhereIf(input.WishedOrFavoriteFilter.HasValue && input.WishedOrFavoriteFilter > -1, e => (input.WishedOrFavoriteFilter == 1 && e.WishedOrFavorite) || (input.WishedOrFavoriteFilter == 0 && !e.WishedOrFavorite))
                        .WhereIf(input.PurchasedFilter.HasValue && input.PurchasedFilter > -1, e => (input.PurchasedFilter == 1 && e.Purchased) || (input.PurchasedFilter == 0 && !e.Purchased))
                        .WhereIf(input.MinPurchasedQuantityFilter != null, e => e.PurchasedQuantity >= input.MinPurchasedQuantityFilter)
                        .WhereIf(input.MaxPurchasedQuantityFilter != null, e => e.PurchasedQuantity <= input.MaxPurchasedQuantityFilter)
                        .WhereIf(input.SharedFilter.HasValue && input.SharedFilter > -1, e => (input.SharedFilter == 1 && e.Shared) || (input.SharedFilter == 0 && !e.Shared))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PageLinkFilter), e => e.PageLink.Contains(input.PageLinkFilter))
                        .WhereIf(input.AppOrWebFilter.HasValue && input.AppOrWebFilter > -1, e => (input.AppOrWebFilter == 1 && e.AppOrWeb) || (input.AppOrWebFilter == 0 && !e.AppOrWeb))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.QuitFromLinkFilter), e => e.QuitFromLink.Contains(input.QuitFromLinkFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SocialMediaNameFilter), e => e.SocialMediaFk != null && e.SocialMediaFk.Name == input.SocialMediaNameFilter);

            var pagedAndFilteredProductCustomerStats = filteredProductCustomerStats
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productCustomerStats = from o in pagedAndFilteredProductCustomerStats
                                       join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                       from s1 in j1.DefaultIfEmpty()

                                       join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                                       from s2 in j2.DefaultIfEmpty()

                                       join o3 in _lookup_storeRepository.GetAll() on o.StoreId equals o3.Id into j3
                                       from s3 in j3.DefaultIfEmpty()

                                       join o4 in _lookup_hubRepository.GetAll() on o.HubId equals o4.Id into j4
                                       from s4 in j4.DefaultIfEmpty()

                                       join o5 in _lookup_socialMediaRepository.GetAll() on o.SocialMediaId equals o5.Id into j5
                                       from s5 in j5.DefaultIfEmpty()

                                       select new
                                       {

                                           o.Clicked,
                                           o.WishedOrFavorite,
                                           o.Purchased,
                                           o.PurchasedQuantity,
                                           o.Shared,
                                           o.PageLink,
                                           o.AppOrWeb,
                                           o.QuitFromLink,
                                           Id = o.Id,
                                           ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                           ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString(),
                                           StoreName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                           HubName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                                           SocialMediaName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                                       };

            var totalCount = await filteredProductCustomerStats.CountAsync();

            var dbList = await productCustomerStats.ToListAsync();
            var results = new List<GetProductCustomerStatForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductCustomerStatForViewDto()
                {
                    ProductCustomerStat = new ProductCustomerStatDto
                    {

                        Clicked = o.Clicked,
                        WishedOrFavorite = o.WishedOrFavorite,
                        Purchased = o.Purchased,
                        PurchasedQuantity = o.PurchasedQuantity,
                        Shared = o.Shared,
                        PageLink = o.PageLink,
                        AppOrWeb = o.AppOrWeb,
                        QuitFromLink = o.QuitFromLink,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    ContactFullName = o.ContactFullName,
                    StoreName = o.StoreName,
                    HubName = o.HubName,
                    SocialMediaName = o.SocialMediaName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductCustomerStatForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductCustomerStatForViewDto> GetProductCustomerStatForView(long id)
        {
            var productCustomerStat = await _productCustomerStatRepository.GetAsync(id);

            var output = new GetProductCustomerStatForViewDto { ProductCustomerStat = ObjectMapper.Map<ProductCustomerStatDto>(productCustomerStat) };

            if (output.ProductCustomerStat.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductCustomerStat.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductCustomerStat.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ProductCustomerStat.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ProductCustomerStat.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.ProductCustomerStat.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.ProductCustomerStat.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.ProductCustomerStat.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.ProductCustomerStat.SocialMediaId != null)
            {
                var _lookupSocialMedia = await _lookup_socialMediaRepository.FirstOrDefaultAsync((long)output.ProductCustomerStat.SocialMediaId);
                output.SocialMediaName = _lookupSocialMedia?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerStats_Edit)]
        public async Task<GetProductCustomerStatForEditOutput> GetProductCustomerStatForEdit(EntityDto<long> input)
        {
            var productCustomerStat = await _productCustomerStatRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductCustomerStatForEditOutput { ProductCustomerStat = ObjectMapper.Map<CreateOrEditProductCustomerStatDto>(productCustomerStat) };

            if (output.ProductCustomerStat.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductCustomerStat.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductCustomerStat.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ProductCustomerStat.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ProductCustomerStat.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.ProductCustomerStat.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.ProductCustomerStat.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.ProductCustomerStat.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.ProductCustomerStat.SocialMediaId != null)
            {
                var _lookupSocialMedia = await _lookup_socialMediaRepository.FirstOrDefaultAsync((long)output.ProductCustomerStat.SocialMediaId);
                output.SocialMediaName = _lookupSocialMedia?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductCustomerStatDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerStats_Create)]
        protected virtual async Task Create(CreateOrEditProductCustomerStatDto input)
        {
            var productCustomerStat = ObjectMapper.Map<ProductCustomerStat>(input);

            if (AbpSession.TenantId != null)
            {
                productCustomerStat.TenantId = (int?)AbpSession.TenantId;
            }

            await _productCustomerStatRepository.InsertAsync(productCustomerStat);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerStats_Edit)]
        protected virtual async Task Update(CreateOrEditProductCustomerStatDto input)
        {
            var productCustomerStat = await _productCustomerStatRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productCustomerStat);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerStats_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productCustomerStatRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductCustomerStatsToExcel(GetAllProductCustomerStatsForExcelInput input)
        {

            var filteredProductCustomerStats = _productCustomerStatRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.HubFk)
                        .Include(e => e.SocialMediaFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PageLink.Contains(input.Filter) || e.QuitFromLink.Contains(input.Filter))
                        .WhereIf(input.ClickedFilter.HasValue && input.ClickedFilter > -1, e => (input.ClickedFilter == 1 && e.Clicked) || (input.ClickedFilter == 0 && !e.Clicked))
                        .WhereIf(input.WishedOrFavoriteFilter.HasValue && input.WishedOrFavoriteFilter > -1, e => (input.WishedOrFavoriteFilter == 1 && e.WishedOrFavorite) || (input.WishedOrFavoriteFilter == 0 && !e.WishedOrFavorite))
                        .WhereIf(input.PurchasedFilter.HasValue && input.PurchasedFilter > -1, e => (input.PurchasedFilter == 1 && e.Purchased) || (input.PurchasedFilter == 0 && !e.Purchased))
                        .WhereIf(input.MinPurchasedQuantityFilter != null, e => e.PurchasedQuantity >= input.MinPurchasedQuantityFilter)
                        .WhereIf(input.MaxPurchasedQuantityFilter != null, e => e.PurchasedQuantity <= input.MaxPurchasedQuantityFilter)
                        .WhereIf(input.SharedFilter.HasValue && input.SharedFilter > -1, e => (input.SharedFilter == 1 && e.Shared) || (input.SharedFilter == 0 && !e.Shared))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PageLinkFilter), e => e.PageLink.Contains(input.PageLinkFilter))
                        .WhereIf(input.AppOrWebFilter.HasValue && input.AppOrWebFilter > -1, e => (input.AppOrWebFilter == 1 && e.AppOrWeb) || (input.AppOrWebFilter == 0 && !e.AppOrWeb))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.QuitFromLinkFilter), e => e.QuitFromLink.Contains(input.QuitFromLinkFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SocialMediaNameFilter), e => e.SocialMediaFk != null && e.SocialMediaFk.Name == input.SocialMediaNameFilter);

            var query = (from o in filteredProductCustomerStats
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_storeRepository.GetAll() on o.StoreId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_hubRepository.GetAll() on o.HubId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_socialMediaRepository.GetAll() on o.SocialMediaId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         select new GetProductCustomerStatForViewDto()
                         {
                             ProductCustomerStat = new ProductCustomerStatDto
                             {
                                 Clicked = o.Clicked,
                                 WishedOrFavorite = o.WishedOrFavorite,
                                 Purchased = o.Purchased,
                                 PurchasedQuantity = o.PurchasedQuantity,
                                 Shared = o.Shared,
                                 PageLink = o.PageLink,
                                 AppOrWeb = o.AppOrWeb,
                                 QuitFromLink = o.QuitFromLink,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString(),
                             StoreName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             HubName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                             SocialMediaName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                         });

            var productCustomerStatListDtos = await query.ToListAsync();

            return _productCustomerStatsExcelExporter.ExportToFile(productCustomerStatListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerStats)]
        public async Task<PagedResultDto<ProductCustomerStatProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCustomerStatProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductCustomerStatProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductCustomerStatProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerStats)]
        public async Task<PagedResultDto<ProductCustomerStatContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCustomerStatContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new ProductCustomerStatContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<ProductCustomerStatContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerStats)]
        public async Task<PagedResultDto<ProductCustomerStatStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCustomerStatStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new ProductCustomerStatStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductCustomerStatStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerStats)]
        public async Task<PagedResultDto<ProductCustomerStatHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_hubRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var hubList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCustomerStatHubLookupTableDto>();
            foreach (var hub in hubList)
            {
                lookupTableDtoList.Add(new ProductCustomerStatHubLookupTableDto
                {
                    Id = hub.Id,
                    DisplayName = hub.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductCustomerStatHubLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerStats)]
        public async Task<PagedResultDto<ProductCustomerStatSocialMediaLookupTableDto>> GetAllSocialMediaForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_socialMediaRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var socialMediaList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCustomerStatSocialMediaLookupTableDto>();
            foreach (var socialMedia in socialMediaList)
            {
                lookupTableDtoList.Add(new ProductCustomerStatSocialMediaLookupTableDto
                {
                    Id = socialMedia.Id,
                    DisplayName = socialMedia.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductCustomerStatSocialMediaLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}