using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.CRM;
using SoftGrid.OrderManagement;
using SoftGrid.LookupData;
using SoftGrid.Shop;
using SoftGrid.OrderManagement;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.OrderManagement.Exporting;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.OrderManagement
{
    [AbpAuthorize(AppPermissions.Pages_Orders)]
    public class OrdersAppService : SoftGridAppServiceBase, IOrdersAppService
    {
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IOrdersExcelExporter _ordersExcelExporter;
        private readonly IRepository<State, long> _lookup_stateRepository;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<OrderStatus, long> _lookup_orderStatusRepository;
        private readonly IRepository<Currency, long> _lookup_currencyRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<OrderSalesChannel, long> _lookup_orderSalesChannelRepository;

        public OrdersAppService(IRepository<Order, long> orderRepository, IOrdersExcelExporter ordersExcelExporter, IRepository<State, long> lookup_stateRepository, IRepository<Country, long> lookup_countryRepository, IRepository<Contact, long> lookup_contactRepository, IRepository<OrderStatus, long> lookup_orderStatusRepository, IRepository<Currency, long> lookup_currencyRepository, IRepository<Store, long> lookup_storeRepository, IRepository<OrderSalesChannel, long> lookup_orderSalesChannelRepository)
        {
            _orderRepository = orderRepository;
            _ordersExcelExporter = ordersExcelExporter;
            _lookup_stateRepository = lookup_stateRepository;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_orderStatusRepository = lookup_orderStatusRepository;
            _lookup_currencyRepository = lookup_currencyRepository;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_orderSalesChannelRepository = lookup_orderSalesChannelRepository;

        }

        public async Task<PagedResultDto<GetOrderForViewDto>> GetAll(GetAllOrdersInput input)
        {

            var filteredOrders = _orderRepository.GetAll()
                        .Include(e => e.StateFk)
                        .Include(e => e.CountryFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.OrderStatusFk)
                        .Include(e => e.CurrencyFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.OrderSalesChannelFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.InvoiceNumber.Contains(input.Filter) || e.FullName.Contains(input.Filter) || e.DeliveryAddress.Contains(input.Filter) || e.City.Contains(input.Filter) || e.ZipCode.Contains(input.Filter) || e.Notes.Contains(input.Filter) || e.Email.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceNumberFilter), e => e.InvoiceNumber.Contains(input.InvoiceNumberFilter))
                        .WhereIf(input.DeliveryOrPickupFilter.HasValue && input.DeliveryOrPickupFilter > -1, e => (input.DeliveryOrPickupFilter == 1 && e.DeliveryOrPickup) || (input.DeliveryOrPickupFilter == 0 && !e.DeliveryOrPickup))
                        .WhereIf(input.PaymentCompletedFilter.HasValue && input.PaymentCompletedFilter > -1, e => (input.PaymentCompletedFilter == 1 && e.PaymentCompleted) || (input.PaymentCompletedFilter == 0 && !e.PaymentCompleted))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullNameFilter), e => e.FullName.Contains(input.FullNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryAddressFilter), e => e.DeliveryAddress.Contains(input.DeliveryAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                        .WhereIf(input.MinDeliveryFeeFilter != null, e => e.DeliveryFee >= input.MinDeliveryFeeFilter)
                        .WhereIf(input.MaxDeliveryFeeFilter != null, e => e.DeliveryFee <= input.MaxDeliveryFeeFilter)
                        .WhereIf(input.MinSubTotalExcludedTaxFilter != null, e => e.SubTotalExcludedTax >= input.MinSubTotalExcludedTaxFilter)
                        .WhereIf(input.MaxSubTotalExcludedTaxFilter != null, e => e.SubTotalExcludedTax <= input.MaxSubTotalExcludedTaxFilter)
                        .WhereIf(input.MinTotalDiscountAmountFilter != null, e => e.TotalDiscountAmount >= input.MinTotalDiscountAmountFilter)
                        .WhereIf(input.MaxTotalDiscountAmountFilter != null, e => e.TotalDiscountAmount <= input.MaxTotalDiscountAmountFilter)
                        .WhereIf(input.MinTotalTaxAmountFilter != null, e => e.TotalTaxAmount >= input.MinTotalTaxAmountFilter)
                        .WhereIf(input.MaxTotalTaxAmountFilter != null, e => e.TotalTaxAmount <= input.MaxTotalTaxAmountFilter)
                        .WhereIf(input.MinTotalAmountFilter != null, e => e.TotalAmount >= input.MinTotalAmountFilter)
                        .WhereIf(input.MaxTotalAmountFilter != null, e => e.TotalAmount <= input.MaxTotalAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(input.MinDiscountAmountByCodeFilter != null, e => e.DiscountAmountByCode >= input.MinDiscountAmountByCodeFilter)
                        .WhereIf(input.MaxDiscountAmountByCodeFilter != null, e => e.DiscountAmountByCode <= input.MaxDiscountAmountByCodeFilter)
                        .WhereIf(input.MinGratuityAmountFilter != null, e => e.GratuityAmount >= input.MinGratuityAmountFilter)
                        .WhereIf(input.MaxGratuityAmountFilter != null, e => e.GratuityAmount <= input.MaxGratuityAmountFilter)
                        .WhereIf(input.MinGratuityPercentageFilter != null, e => e.GratuityPercentage >= input.MinGratuityPercentageFilter)
                        .WhereIf(input.MaxGratuityPercentageFilter != null, e => e.GratuityPercentage <= input.MaxGratuityPercentageFilter)
                        .WhereIf(input.MinServiceChargeFilter != null, e => e.ServiceCharge >= input.MinServiceChargeFilter)
                        .WhereIf(input.MaxServiceChargeFilter != null, e => e.ServiceCharge <= input.MaxServiceChargeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderStatusNameFilter), e => e.OrderStatusFk != null && e.OrderStatusFk.Name == input.OrderStatusNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderSalesChannelNameFilter), e => e.OrderSalesChannelFk != null && e.OrderSalesChannelFk.Name == input.OrderSalesChannelNameFilter);

            var pagedAndFilteredOrders = filteredOrders
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orders = from o in pagedAndFilteredOrders
                         join o1 in _lookup_stateRepository.GetAll() on o.StateId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_countryRepository.GetAll() on o.CountryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_contactRepository.GetAll() on o.ContactId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_orderStatusRepository.GetAll() on o.OrderStatusId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         join o6 in _lookup_storeRepository.GetAll() on o.StoreId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()

                         join o7 in _lookup_orderSalesChannelRepository.GetAll() on o.OrderSalesChannelId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()

                         select new
                         {

                             o.InvoiceNumber,
                             o.DeliveryOrPickup,
                             o.PaymentCompleted,
                             o.FullName,
                             o.DeliveryAddress,
                             o.City,
                             o.ZipCode,
                             o.Notes,
                             o.DeliveryFee,
                             o.SubTotalExcludedTax,
                             o.TotalDiscountAmount,
                             o.TotalTaxAmount,
                             o.TotalAmount,
                             o.Email,
                             o.DiscountAmountByCode,
                             o.GratuityAmount,
                             o.GratuityPercentage,
                             o.ServiceCharge,
                             Id = o.Id,
                             StateName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             CountryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ContactFullName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString(),
                             OrderStatusName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                             CurrencyName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                             StoreName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
                             OrderSalesChannelName = s7 == null || s7.Name == null ? "" : s7.Name.ToString()
                         };

            var totalCount = await filteredOrders.CountAsync();

            var dbList = await orders.ToListAsync();
            var results = new List<GetOrderForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderForViewDto()
                {
                    Order = new OrderDto
                    {

                        InvoiceNumber = o.InvoiceNumber,
                        DeliveryOrPickup = o.DeliveryOrPickup,
                        PaymentCompleted = o.PaymentCompleted,
                        FullName = o.FullName,
                        DeliveryAddress = o.DeliveryAddress,
                        City = o.City,
                        ZipCode = o.ZipCode,
                        Notes = o.Notes,
                        DeliveryFee = o.DeliveryFee,
                        SubTotalExcludedTax = o.SubTotalExcludedTax,
                        TotalDiscountAmount = o.TotalDiscountAmount,
                        TotalTaxAmount = o.TotalTaxAmount,
                        TotalAmount = o.TotalAmount,
                        Email = o.Email,
                        DiscountAmountByCode = o.DiscountAmountByCode,
                        GratuityAmount = o.GratuityAmount,
                        GratuityPercentage = o.GratuityPercentage,
                        ServiceCharge = o.ServiceCharge,
                        Id = o.Id,
                    },
                    StateName = o.StateName,
                    CountryName = o.CountryName,
                    ContactFullName = o.ContactFullName,
                    OrderStatusName = o.OrderStatusName,
                    CurrencyName = o.CurrencyName,
                    StoreName = o.StoreName,
                    OrderSalesChannelName = o.OrderSalesChannelName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOrderForViewDto> GetOrderForView(long id)
        {
            var order = await _orderRepository.GetAsync(id);

            var output = new GetOrderForViewDto { Order = ObjectMapper.Map<OrderDto>(order) };

            if (output.Order.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.Order.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.Order.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Order.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Order.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.Order.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.Order.OrderStatusId != null)
            {
                var _lookupOrderStatus = await _lookup_orderStatusRepository.FirstOrDefaultAsync((long)output.Order.OrderStatusId);
                output.OrderStatusName = _lookupOrderStatus?.Name?.ToString();
            }

            if (output.Order.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.Order.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            if (output.Order.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.Order.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.Order.OrderSalesChannelId != null)
            {
                var _lookupOrderSalesChannel = await _lookup_orderSalesChannelRepository.FirstOrDefaultAsync((long)output.Order.OrderSalesChannelId);
                output.OrderSalesChannelName = _lookupOrderSalesChannel?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Orders_Edit)]
        public async Task<GetOrderForEditOutput> GetOrderForEdit(EntityDto<long> input)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderForEditOutput { Order = ObjectMapper.Map<CreateOrEditOrderDto>(order) };

            if (output.Order.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.Order.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.Order.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Order.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Order.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.Order.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.Order.OrderStatusId != null)
            {
                var _lookupOrderStatus = await _lookup_orderStatusRepository.FirstOrDefaultAsync((long)output.Order.OrderStatusId);
                output.OrderStatusName = _lookupOrderStatus?.Name?.ToString();
            }

            if (output.Order.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.Order.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            if (output.Order.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.Order.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.Order.OrderSalesChannelId != null)
            {
                var _lookupOrderSalesChannel = await _lookup_orderSalesChannelRepository.FirstOrDefaultAsync((long)output.Order.OrderSalesChannelId);
                output.OrderSalesChannelName = _lookupOrderSalesChannel?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrderDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Orders_Create)]
        protected virtual async Task Create(CreateOrEditOrderDto input)
        {
            var order = ObjectMapper.Map<Order>(input);

            if (AbpSession.TenantId != null)
            {
                order.TenantId = (int?)AbpSession.TenantId;
            }

            await _orderRepository.InsertAsync(order);

        }

        [AbpAuthorize(AppPermissions.Pages_Orders_Edit)]
        protected virtual async Task Update(CreateOrEditOrderDto input)
        {
            var order = await _orderRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, order);

        }

        [AbpAuthorize(AppPermissions.Pages_Orders_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _orderRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOrdersToExcel(GetAllOrdersForExcelInput input)
        {

            var filteredOrders = _orderRepository.GetAll()
                        .Include(e => e.StateFk)
                        .Include(e => e.CountryFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.OrderStatusFk)
                        .Include(e => e.CurrencyFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.OrderSalesChannelFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.InvoiceNumber.Contains(input.Filter) || e.FullName.Contains(input.Filter) || e.DeliveryAddress.Contains(input.Filter) || e.City.Contains(input.Filter) || e.ZipCode.Contains(input.Filter) || e.Notes.Contains(input.Filter) || e.Email.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvoiceNumberFilter), e => e.InvoiceNumber.Contains(input.InvoiceNumberFilter))
                        .WhereIf(input.DeliveryOrPickupFilter.HasValue && input.DeliveryOrPickupFilter > -1, e => (input.DeliveryOrPickupFilter == 1 && e.DeliveryOrPickup) || (input.DeliveryOrPickupFilter == 0 && !e.DeliveryOrPickup))
                        .WhereIf(input.PaymentCompletedFilter.HasValue && input.PaymentCompletedFilter > -1, e => (input.PaymentCompletedFilter == 1 && e.PaymentCompleted) || (input.PaymentCompletedFilter == 0 && !e.PaymentCompleted))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullNameFilter), e => e.FullName.Contains(input.FullNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryAddressFilter), e => e.DeliveryAddress.Contains(input.DeliveryAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                        .WhereIf(input.MinDeliveryFeeFilter != null, e => e.DeliveryFee >= input.MinDeliveryFeeFilter)
                        .WhereIf(input.MaxDeliveryFeeFilter != null, e => e.DeliveryFee <= input.MaxDeliveryFeeFilter)
                        .WhereIf(input.MinSubTotalExcludedTaxFilter != null, e => e.SubTotalExcludedTax >= input.MinSubTotalExcludedTaxFilter)
                        .WhereIf(input.MaxSubTotalExcludedTaxFilter != null, e => e.SubTotalExcludedTax <= input.MaxSubTotalExcludedTaxFilter)
                        .WhereIf(input.MinTotalDiscountAmountFilter != null, e => e.TotalDiscountAmount >= input.MinTotalDiscountAmountFilter)
                        .WhereIf(input.MaxTotalDiscountAmountFilter != null, e => e.TotalDiscountAmount <= input.MaxTotalDiscountAmountFilter)
                        .WhereIf(input.MinTotalTaxAmountFilter != null, e => e.TotalTaxAmount >= input.MinTotalTaxAmountFilter)
                        .WhereIf(input.MaxTotalTaxAmountFilter != null, e => e.TotalTaxAmount <= input.MaxTotalTaxAmountFilter)
                        .WhereIf(input.MinTotalAmountFilter != null, e => e.TotalAmount >= input.MinTotalAmountFilter)
                        .WhereIf(input.MaxTotalAmountFilter != null, e => e.TotalAmount <= input.MaxTotalAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(input.MinDiscountAmountByCodeFilter != null, e => e.DiscountAmountByCode >= input.MinDiscountAmountByCodeFilter)
                        .WhereIf(input.MaxDiscountAmountByCodeFilter != null, e => e.DiscountAmountByCode <= input.MaxDiscountAmountByCodeFilter)
                        .WhereIf(input.MinGratuityAmountFilter != null, e => e.GratuityAmount >= input.MinGratuityAmountFilter)
                        .WhereIf(input.MaxGratuityAmountFilter != null, e => e.GratuityAmount <= input.MaxGratuityAmountFilter)
                        .WhereIf(input.MinGratuityPercentageFilter != null, e => e.GratuityPercentage >= input.MinGratuityPercentageFilter)
                        .WhereIf(input.MaxGratuityPercentageFilter != null, e => e.GratuityPercentage <= input.MaxGratuityPercentageFilter)
                        .WhereIf(input.MinServiceChargeFilter != null, e => e.ServiceCharge >= input.MinServiceChargeFilter)
                        .WhereIf(input.MaxServiceChargeFilter != null, e => e.ServiceCharge <= input.MaxServiceChargeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderStatusNameFilter), e => e.OrderStatusFk != null && e.OrderStatusFk.Name == input.OrderStatusNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderSalesChannelNameFilter), e => e.OrderSalesChannelFk != null && e.OrderSalesChannelFk.Name == input.OrderSalesChannelNameFilter);

            var query = (from o in filteredOrders
                         join o1 in _lookup_stateRepository.GetAll() on o.StateId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_countryRepository.GetAll() on o.CountryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_contactRepository.GetAll() on o.ContactId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_orderStatusRepository.GetAll() on o.OrderStatusId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         join o6 in _lookup_storeRepository.GetAll() on o.StoreId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()

                         join o7 in _lookup_orderSalesChannelRepository.GetAll() on o.OrderSalesChannelId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()

                         select new GetOrderForViewDto()
                         {
                             Order = new OrderDto
                             {
                                 InvoiceNumber = o.InvoiceNumber,
                                 DeliveryOrPickup = o.DeliveryOrPickup,
                                 PaymentCompleted = o.PaymentCompleted,
                                 FullName = o.FullName,
                                 DeliveryAddress = o.DeliveryAddress,
                                 City = o.City,
                                 ZipCode = o.ZipCode,
                                 Notes = o.Notes,
                                 DeliveryFee = o.DeliveryFee,
                                 SubTotalExcludedTax = o.SubTotalExcludedTax,
                                 TotalDiscountAmount = o.TotalDiscountAmount,
                                 TotalTaxAmount = o.TotalTaxAmount,
                                 TotalAmount = o.TotalAmount,
                                 Email = o.Email,
                                 DiscountAmountByCode = o.DiscountAmountByCode,
                                 GratuityAmount = o.GratuityAmount,
                                 GratuityPercentage = o.GratuityPercentage,
                                 ServiceCharge = o.ServiceCharge,
                                 Id = o.Id
                             },
                             StateName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             CountryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ContactFullName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString(),
                             OrderStatusName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                             CurrencyName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                             StoreName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
                             OrderSalesChannelName = s7 == null || s7.Name == null ? "" : s7.Name.ToString()
                         });

            var orderListDtos = await query.ToListAsync();

            return _ordersExcelExporter.ExportToFile(orderListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Orders)]
        public async Task<PagedResultDto<OrderStateLookupTableDto>> GetAllStateForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_stateRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var stateList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderStateLookupTableDto>();
            foreach (var state in stateList)
            {
                lookupTableDtoList.Add(new OrderStateLookupTableDto
                {
                    Id = state.Id,
                    DisplayName = state.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderStateLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Orders)]
        public async Task<PagedResultDto<OrderCountryLookupTableDto>> GetAllCountryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_countryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var countryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderCountryLookupTableDto>();
            foreach (var country in countryList)
            {
                lookupTableDtoList.Add(new OrderCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderCountryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Orders)]
        public async Task<PagedResultDto<OrderContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new OrderContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<OrderContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Orders)]
        public async Task<PagedResultDto<OrderOrderStatusLookupTableDto>> GetAllOrderStatusForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_orderStatusRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var orderStatusList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderOrderStatusLookupTableDto>();
            foreach (var orderStatus in orderStatusList)
            {
                lookupTableDtoList.Add(new OrderOrderStatusLookupTableDto
                {
                    Id = orderStatus.Id,
                    DisplayName = orderStatus.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderOrderStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Orders)]
        public async Task<PagedResultDto<OrderCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_currencyRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var currencyList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderCurrencyLookupTableDto>();
            foreach (var currency in currencyList)
            {
                lookupTableDtoList.Add(new OrderCurrencyLookupTableDto
                {
                    Id = currency.Id,
                    DisplayName = currency.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderCurrencyLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Orders)]
        public async Task<PagedResultDto<OrderStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new OrderStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Orders)]
        public async Task<PagedResultDto<OrderOrderSalesChannelLookupTableDto>> GetAllOrderSalesChannelForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_orderSalesChannelRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var orderSalesChannelList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderOrderSalesChannelLookupTableDto>();
            foreach (var orderSalesChannel in orderSalesChannelList)
            {
                lookupTableDtoList.Add(new OrderOrderSalesChannelLookupTableDto
                {
                    Id = orderSalesChannel.Id,
                    DisplayName = orderSalesChannel.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderOrderSalesChannelLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}