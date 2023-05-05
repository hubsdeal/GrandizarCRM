using SoftGrid.CRM;
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
    [AbpAuthorize(AppPermissions.Pages_WishLists)]
    public class WishListsAppService : SoftGridAppServiceBase, IWishListsAppService
    {
        private readonly IRepository<WishList, long> _wishListRepository;
        private readonly IWishListsExcelExporter _wishListsExcelExporter;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;

        public WishListsAppService(IRepository<WishList, long> wishListRepository, IWishListsExcelExporter wishListsExcelExporter, IRepository<Contact, long> lookup_contactRepository, IRepository<Product, long> lookup_productRepository, IRepository<Store, long> lookup_storeRepository)
        {
            _wishListRepository = wishListRepository;
            _wishListsExcelExporter = wishListsExcelExporter;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_storeRepository = lookup_storeRepository;

        }

        public async Task<PagedResultDto<GetWishListForViewDto>> GetAll(GetAllWishListsInput input)
        {

            var filteredWishLists = _wishListRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.ProductFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDateFilter != null, e => e.Date >= input.MinDateFilter)
                        .WhereIf(input.MaxDateFilter != null, e => e.Date <= input.MaxDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var pagedAndFilteredWishLists = filteredWishLists
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var wishLists = from o in pagedAndFilteredWishLists
                            join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            join o3 in _lookup_storeRepository.GetAll() on o.StoreId equals o3.Id into j3
                            from s3 in j3.DefaultIfEmpty()

                            select new
                            {

                                o.Date,
                                Id = o.Id,
                                ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                                ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                StoreName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                            };

            var totalCount = await filteredWishLists.CountAsync();

            var dbList = await wishLists.ToListAsync();
            var results = new List<GetWishListForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetWishListForViewDto()
                {
                    WishList = new WishListDto
                    {

                        Date = o.Date,
                        Id = o.Id,
                    },
                    ContactFullName = o.ContactFullName,
                    ProductName = o.ProductName,
                    StoreName = o.StoreName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetWishListForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetWishListForViewDto> GetWishListForView(long id)
        {
            var wishList = await _wishListRepository.GetAsync(id);

            var output = new GetWishListForViewDto { WishList = ObjectMapper.Map<WishListDto>(wishList) };

            if (output.WishList.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.WishList.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.WishList.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.WishList.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.WishList.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.WishList.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_WishLists_Edit)]
        public async Task<GetWishListForEditOutput> GetWishListForEdit(EntityDto<long> input)
        {
            var wishList = await _wishListRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetWishListForEditOutput { WishList = ObjectMapper.Map<CreateOrEditWishListDto>(wishList) };

            if (output.WishList.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.WishList.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.WishList.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.WishList.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.WishList.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.WishList.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditWishListDto input)
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

        [AbpAuthorize(AppPermissions.Pages_WishLists_Create)]
        protected virtual async Task Create(CreateOrEditWishListDto input)
        {
            var wishList = ObjectMapper.Map<WishList>(input);

            if (AbpSession.TenantId != null)
            {
                wishList.TenantId = (int?)AbpSession.TenantId;
            }

            await _wishListRepository.InsertAsync(wishList);

        }

        [AbpAuthorize(AppPermissions.Pages_WishLists_Edit)]
        protected virtual async Task Update(CreateOrEditWishListDto input)
        {
            var wishList = await _wishListRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, wishList);

        }

        [AbpAuthorize(AppPermissions.Pages_WishLists_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _wishListRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetWishListsToExcel(GetAllWishListsForExcelInput input)
        {

            var filteredWishLists = _wishListRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.ProductFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDateFilter != null, e => e.Date >= input.MinDateFilter)
                        .WhereIf(input.MaxDateFilter != null, e => e.Date <= input.MaxDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var query = (from o in filteredWishLists
                         join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_storeRepository.GetAll() on o.StoreId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetWishListForViewDto()
                         {
                             WishList = new WishListDto
                             {
                                 Date = o.Date,
                                 Id = o.Id
                             },
                             ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                             ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             StoreName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var wishListListDtos = await query.ToListAsync();

            return _wishListsExcelExporter.ExportToFile(wishListListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_WishLists)]
        public async Task<PagedResultDto<WishListContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<WishListContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new WishListContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<WishListContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_WishLists)]
        public async Task<PagedResultDto<WishListProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<WishListProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new WishListProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<WishListProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_WishLists)]
        public async Task<PagedResultDto<WishListStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<WishListStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new WishListStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<WishListStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}