using SoftGrid.CRM;
using SoftGrid.OrderManagement;
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
    [AbpAuthorize(AppPermissions.Pages_ShoppingCarts)]
    public class ShoppingCartsAppService : SoftGridAppServiceBase, IShoppingCartsAppService
    {
        private readonly IRepository<ShoppingCart, long> _shoppingCartRepository;
        private readonly IShoppingCartsExcelExporter _shoppingCartsExcelExporter;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<Order, long> _lookup_orderRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<Currency, long> _lookup_currencyRepository;

        public ShoppingCartsAppService(IRepository<ShoppingCart, long> shoppingCartRepository, IShoppingCartsExcelExporter shoppingCartsExcelExporter, IRepository<Contact, long> lookup_contactRepository, IRepository<Order, long> lookup_orderRepository, IRepository<Store, long> lookup_storeRepository, IRepository<Product, long> lookup_productRepository, IRepository<Currency, long> lookup_currencyRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartsExcelExporter = shoppingCartsExcelExporter;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_orderRepository = lookup_orderRepository;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_currencyRepository = lookup_currencyRepository;

        }

        public async Task<PagedResultDto<GetShoppingCartForViewDto>> GetAll(GetAllShoppingCartsInput input)
        {

            var filteredShoppingCarts = _shoppingCartRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.OrderFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.ProductFk)
                        .Include(e => e.CurrencyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(input.MinUnitPriceFilter != null, e => e.UnitPrice >= input.MinUnitPriceFilter)
                        .WhereIf(input.MaxUnitPriceFilter != null, e => e.UnitPrice <= input.MaxUnitPriceFilter)
                        .WhereIf(input.MinTotalAmountFilter != null, e => e.TotalAmount >= input.MinTotalAmountFilter)
                        .WhereIf(input.MaxTotalAmountFilter != null, e => e.TotalAmount <= input.MaxTotalAmountFilter)
                        .WhereIf(input.MinUnitTotalPriceFilter != null, e => e.UnitTotalPrice >= input.MinUnitTotalPriceFilter)
                        .WhereIf(input.MaxUnitTotalPriceFilter != null, e => e.UnitTotalPrice <= input.MaxUnitTotalPriceFilter)
                        .WhereIf(input.MinUnitDiscountAmountFilter != null, e => e.UnitDiscountAmount >= input.MinUnitDiscountAmountFilter)
                        .WhereIf(input.MaxUnitDiscountAmountFilter != null, e => e.UnitDiscountAmount <= input.MaxUnitDiscountAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderInvoiceNumberFilter), e => e.OrderFk != null && e.OrderFk.InvoiceNumber == input.OrderInvoiceNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter);

            var pagedAndFilteredShoppingCarts = filteredShoppingCarts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var shoppingCarts = from o in pagedAndFilteredShoppingCarts
                                join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _lookup_orderRepository.GetAll() on o.OrderId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                join o3 in _lookup_storeRepository.GetAll() on o.StoreId equals o3.Id into j3
                                from s3 in j3.DefaultIfEmpty()

                                join o4 in _lookup_productRepository.GetAll() on o.ProductId equals o4.Id into j4
                                from s4 in j4.DefaultIfEmpty()

                                join o5 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o5.Id into j5
                                from s5 in j5.DefaultIfEmpty()

                                select new
                                {

                                    o.Quantity,
                                    o.UnitPrice,
                                    o.TotalAmount,
                                    o.UnitTotalPrice,
                                    o.UnitDiscountAmount,
                                    Id = o.Id,
                                    ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                                    OrderInvoiceNumber = s2 == null || s2.InvoiceNumber == null ? "" : s2.InvoiceNumber.ToString(),
                                    StoreName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                    ProductName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                                    CurrencyName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                                };

            var totalCount = await filteredShoppingCarts.CountAsync();

            var dbList = await shoppingCarts.ToListAsync();
            var results = new List<GetShoppingCartForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetShoppingCartForViewDto()
                {
                    ShoppingCart = new ShoppingCartDto
                    {

                        Quantity = o.Quantity,
                        UnitPrice = o.UnitPrice,
                        TotalAmount = o.TotalAmount,
                        UnitTotalPrice = o.UnitTotalPrice,
                        UnitDiscountAmount = o.UnitDiscountAmount,
                        Id = o.Id,
                    },
                    ContactFullName = o.ContactFullName,
                    OrderInvoiceNumber = o.OrderInvoiceNumber,
                    StoreName = o.StoreName,
                    ProductName = o.ProductName,
                    CurrencyName = o.CurrencyName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetShoppingCartForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetShoppingCartForViewDto> GetShoppingCartForView(long id)
        {
            var shoppingCart = await _shoppingCartRepository.GetAsync(id);

            var output = new GetShoppingCartForViewDto { ShoppingCart = ObjectMapper.Map<ShoppingCartDto>(shoppingCart) };

            if (output.ShoppingCart.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ShoppingCart.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ShoppingCart.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.ShoppingCart.OrderId);
                output.OrderInvoiceNumber = _lookupOrder?.InvoiceNumber?.ToString();
            }

            if (output.ShoppingCart.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.ShoppingCart.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.ShoppingCart.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ShoppingCart.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ShoppingCart.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.ShoppingCart.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ShoppingCarts_Edit)]
        public async Task<GetShoppingCartForEditOutput> GetShoppingCartForEdit(EntityDto<long> input)
        {
            var shoppingCart = await _shoppingCartRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetShoppingCartForEditOutput { ShoppingCart = ObjectMapper.Map<CreateOrEditShoppingCartDto>(shoppingCart) };

            if (output.ShoppingCart.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ShoppingCart.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ShoppingCart.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.ShoppingCart.OrderId);
                output.OrderInvoiceNumber = _lookupOrder?.InvoiceNumber?.ToString();
            }

            if (output.ShoppingCart.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.ShoppingCart.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.ShoppingCart.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ShoppingCart.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ShoppingCart.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.ShoppingCart.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditShoppingCartDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ShoppingCarts_Create)]
        protected virtual async Task Create(CreateOrEditShoppingCartDto input)
        {
            var shoppingCart = ObjectMapper.Map<ShoppingCart>(input);

            if (AbpSession.TenantId != null)
            {
                shoppingCart.TenantId = (int?)AbpSession.TenantId;
            }

            await _shoppingCartRepository.InsertAsync(shoppingCart);

        }

        [AbpAuthorize(AppPermissions.Pages_ShoppingCarts_Edit)]
        protected virtual async Task Update(CreateOrEditShoppingCartDto input)
        {
            var shoppingCart = await _shoppingCartRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, shoppingCart);

        }

        [AbpAuthorize(AppPermissions.Pages_ShoppingCarts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _shoppingCartRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetShoppingCartsToExcel(GetAllShoppingCartsForExcelInput input)
        {

            var filteredShoppingCarts = _shoppingCartRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.OrderFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.ProductFk)
                        .Include(e => e.CurrencyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(input.MinUnitPriceFilter != null, e => e.UnitPrice >= input.MinUnitPriceFilter)
                        .WhereIf(input.MaxUnitPriceFilter != null, e => e.UnitPrice <= input.MaxUnitPriceFilter)
                        .WhereIf(input.MinTotalAmountFilter != null, e => e.TotalAmount >= input.MinTotalAmountFilter)
                        .WhereIf(input.MaxTotalAmountFilter != null, e => e.TotalAmount <= input.MaxTotalAmountFilter)
                        .WhereIf(input.MinUnitTotalPriceFilter != null, e => e.UnitTotalPrice >= input.MinUnitTotalPriceFilter)
                        .WhereIf(input.MaxUnitTotalPriceFilter != null, e => e.UnitTotalPrice <= input.MaxUnitTotalPriceFilter)
                        .WhereIf(input.MinUnitDiscountAmountFilter != null, e => e.UnitDiscountAmount >= input.MinUnitDiscountAmountFilter)
                        .WhereIf(input.MaxUnitDiscountAmountFilter != null, e => e.UnitDiscountAmount <= input.MaxUnitDiscountAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderInvoiceNumberFilter), e => e.OrderFk != null && e.OrderFk.InvoiceNumber == input.OrderInvoiceNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter);

            var query = (from o in filteredShoppingCarts
                         join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_orderRepository.GetAll() on o.OrderId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_storeRepository.GetAll() on o.StoreId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_productRepository.GetAll() on o.ProductId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         select new GetShoppingCartForViewDto()
                         {
                             ShoppingCart = new ShoppingCartDto
                             {
                                 Quantity = o.Quantity,
                                 UnitPrice = o.UnitPrice,
                                 TotalAmount = o.TotalAmount,
                                 UnitTotalPrice = o.UnitTotalPrice,
                                 UnitDiscountAmount = o.UnitDiscountAmount,
                                 Id = o.Id
                             },
                             ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                             OrderInvoiceNumber = s2 == null || s2.InvoiceNumber == null ? "" : s2.InvoiceNumber.ToString(),
                             StoreName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             ProductName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                             CurrencyName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                         });

            var shoppingCartListDtos = await query.ToListAsync();

            return _shoppingCartsExcelExporter.ExportToFile(shoppingCartListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ShoppingCarts)]
        public async Task<PagedResultDto<ShoppingCartContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ShoppingCartContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new ShoppingCartContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<ShoppingCartContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ShoppingCarts)]
        public async Task<PagedResultDto<ShoppingCartOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_orderRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.InvoiceNumber != null && e.InvoiceNumber.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var orderList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ShoppingCartOrderLookupTableDto>();
            foreach (var order in orderList)
            {
                lookupTableDtoList.Add(new ShoppingCartOrderLookupTableDto
                {
                    Id = order.Id,
                    DisplayName = order.InvoiceNumber?.ToString()
                });
            }

            return new PagedResultDto<ShoppingCartOrderLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ShoppingCarts)]
        public async Task<PagedResultDto<ShoppingCartStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ShoppingCartStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new ShoppingCartStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<ShoppingCartStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ShoppingCarts)]
        public async Task<PagedResultDto<ShoppingCartProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ShoppingCartProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ShoppingCartProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ShoppingCartProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ShoppingCarts)]
        public async Task<PagedResultDto<ShoppingCartCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_currencyRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var currencyList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ShoppingCartCurrencyLookupTableDto>();
            foreach (var currency in currencyList)
            {
                lookupTableDtoList.Add(new ShoppingCartCurrencyLookupTableDto
                {
                    Id = currency.Id,
                    DisplayName = currency.Name?.ToString()
                });
            }

            return new PagedResultDto<ShoppingCartCurrencyLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}