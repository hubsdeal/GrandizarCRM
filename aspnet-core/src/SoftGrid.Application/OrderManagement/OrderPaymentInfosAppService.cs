using SoftGrid.OrderManagement;
using SoftGrid.LookupData;
using SoftGrid.LookupData;

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
    [AbpAuthorize(AppPermissions.Pages_OrderPaymentInfos)]
    public class OrderPaymentInfosAppService : SoftGridAppServiceBase, IOrderPaymentInfosAppService
    {
        private readonly IRepository<OrderPaymentInfo, long> _orderPaymentInfoRepository;
        private readonly IOrderPaymentInfosExcelExporter _orderPaymentInfosExcelExporter;
        private readonly IRepository<Order, long> _lookup_orderRepository;
        private readonly IRepository<Currency, long> _lookup_currencyRepository;
        private readonly IRepository<PaymentType, long> _lookup_paymentTypeRepository;

        public OrderPaymentInfosAppService(IRepository<OrderPaymentInfo, long> orderPaymentInfoRepository, IOrderPaymentInfosExcelExporter orderPaymentInfosExcelExporter, IRepository<Order, long> lookup_orderRepository, IRepository<Currency, long> lookup_currencyRepository, IRepository<PaymentType, long> lookup_paymentTypeRepository)
        {
            _orderPaymentInfoRepository = orderPaymentInfoRepository;
            _orderPaymentInfosExcelExporter = orderPaymentInfosExcelExporter;
            _lookup_orderRepository = lookup_orderRepository;
            _lookup_currencyRepository = lookup_currencyRepository;
            _lookup_paymentTypeRepository = lookup_paymentTypeRepository;

        }

        public async Task<PagedResultDto<GetOrderPaymentInfoForViewDto>> GetAll(GetAllOrderPaymentInfosInput input)
        {

            var filteredOrderPaymentInfos = _orderPaymentInfoRepository.GetAll()
                        .Include(e => e.OrderFk)
                        .Include(e => e.CurrencyFk)
                        .Include(e => e.PaymentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BillingAddress.Contains(input.Filter) || e.BillingCity.Contains(input.Filter) || e.BillingState.Contains(input.Filter) || e.BillingZipCode.Contains(input.Filter) || e.MaskedCreditCardNumber.Contains(input.Filter) || e.CardName.Contains(input.Filter) || e.CardNumber.Contains(input.Filter) || e.CardCvv.Contains(input.Filter) || e.CardExpirationMonth.Contains(input.Filter) || e.CardExpirationYear.Contains(input.Filter) || e.AuthorizationTransactionNumber.Contains(input.Filter) || e.AuthorizationTransactionCode.Contains(input.Filter) || e.AuthrorizationTransactionResult.Contains(input.Filter) || e.CustomerIpAddress.Contains(input.Filter) || e.CustomerDeviceInfo.Contains(input.Filter) || e.PaidTime.Contains(input.Filter))
                        .WhereIf(input.PaymentSplitFilter.HasValue && input.PaymentSplitFilter > -1, e => (input.PaymentSplitFilter == 1 && e.PaymentSplit) || (input.PaymentSplitFilter == 0 && !e.PaymentSplit))
                        .WhereIf(input.MinDueAmountFilter != null, e => e.DueAmount >= input.MinDueAmountFilter)
                        .WhereIf(input.MaxDueAmountFilter != null, e => e.DueAmount <= input.MaxDueAmountFilter)
                        .WhereIf(input.MinPaySplitAmountFilter != null, e => e.PaySplitAmount >= input.MinPaySplitAmountFilter)
                        .WhereIf(input.MaxPaySplitAmountFilter != null, e => e.PaySplitAmount <= input.MaxPaySplitAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BillingAddressFilter), e => e.BillingAddress.Contains(input.BillingAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BillingCityFilter), e => e.BillingCity.Contains(input.BillingCityFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BillingStateFilter), e => e.BillingState.Contains(input.BillingStateFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BillingZipCodeFilter), e => e.BillingZipCode.Contains(input.BillingZipCodeFilter))
                        .WhereIf(input.SaveCreditCardNumberFilter.HasValue && input.SaveCreditCardNumberFilter > -1, e => (input.SaveCreditCardNumberFilter == 1 && e.SaveCreditCardNumber) || (input.SaveCreditCardNumberFilter == 0 && !e.SaveCreditCardNumber))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MaskedCreditCardNumberFilter), e => e.MaskedCreditCardNumber.Contains(input.MaskedCreditCardNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardNameFilter), e => e.CardName.Contains(input.CardNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardNumberFilter), e => e.CardNumber.Contains(input.CardNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardCvvFilter), e => e.CardCvv.Contains(input.CardCvvFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardExpirationMonthFilter), e => e.CardExpirationMonth.Contains(input.CardExpirationMonthFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardExpirationYearFilter), e => e.CardExpirationYear.Contains(input.CardExpirationYearFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthorizationTransactionNumberFilter), e => e.AuthorizationTransactionNumber.Contains(input.AuthorizationTransactionNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthorizationTransactionCodeFilter), e => e.AuthorizationTransactionCode.Contains(input.AuthorizationTransactionCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthrorizationTransactionResultFilter), e => e.AuthrorizationTransactionResult.Contains(input.AuthrorizationTransactionResultFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIpAddressFilter), e => e.CustomerIpAddress.Contains(input.CustomerIpAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerDeviceInfoFilter), e => e.CustomerDeviceInfo.Contains(input.CustomerDeviceInfoFilter))
                        .WhereIf(input.MinPaidDateFilter != null, e => e.PaidDate >= input.MinPaidDateFilter)
                        .WhereIf(input.MaxPaidDateFilter != null, e => e.PaidDate <= input.MaxPaidDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaidTimeFilter), e => e.PaidTime.Contains(input.PaidTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderInvoiceNumberFilter), e => e.OrderFk != null && e.OrderFk.InvoiceNumber == input.OrderInvoiceNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentTypeNameFilter), e => e.PaymentTypeFk != null && e.PaymentTypeFk.Name == input.PaymentTypeNameFilter);

            var pagedAndFilteredOrderPaymentInfos = filteredOrderPaymentInfos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderPaymentInfos = from o in pagedAndFilteredOrderPaymentInfos
                                    join o1 in _lookup_orderRepository.GetAll() on o.OrderId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()

                                    join o3 in _lookup_paymentTypeRepository.GetAll() on o.PaymentTypeId equals o3.Id into j3
                                    from s3 in j3.DefaultIfEmpty()

                                    select new
                                    {

                                        o.PaymentSplit,
                                        o.DueAmount,
                                        o.PaySplitAmount,
                                        o.BillingAddress,
                                        o.BillingCity,
                                        o.BillingState,
                                        o.BillingZipCode,
                                        o.SaveCreditCardNumber,
                                        o.MaskedCreditCardNumber,
                                        o.CardName,
                                        o.CardNumber,
                                        o.CardCvv,
                                        o.CardExpirationMonth,
                                        o.CardExpirationYear,
                                        o.AuthorizationTransactionNumber,
                                        o.AuthorizationTransactionCode,
                                        o.AuthrorizationTransactionResult,
                                        o.CustomerIpAddress,
                                        o.CustomerDeviceInfo,
                                        o.PaidDate,
                                        o.PaidTime,
                                        Id = o.Id,
                                        OrderInvoiceNumber = s1 == null || s1.InvoiceNumber == null ? "" : s1.InvoiceNumber.ToString(),
                                        CurrencyName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                        PaymentTypeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                    };

            var totalCount = await filteredOrderPaymentInfos.CountAsync();

            var dbList = await orderPaymentInfos.ToListAsync();
            var results = new List<GetOrderPaymentInfoForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderPaymentInfoForViewDto()
                {
                    OrderPaymentInfo = new OrderPaymentInfoDto
                    {

                        PaymentSplit = o.PaymentSplit,
                        DueAmount = o.DueAmount,
                        PaySplitAmount = o.PaySplitAmount,
                        BillingAddress = o.BillingAddress,
                        BillingCity = o.BillingCity,
                        BillingState = o.BillingState,
                        BillingZipCode = o.BillingZipCode,
                        SaveCreditCardNumber = o.SaveCreditCardNumber,
                        MaskedCreditCardNumber = o.MaskedCreditCardNumber,
                        CardName = o.CardName,
                        CardNumber = o.CardNumber,
                        CardCvv = o.CardCvv,
                        CardExpirationMonth = o.CardExpirationMonth,
                        CardExpirationYear = o.CardExpirationYear,
                        AuthorizationTransactionNumber = o.AuthorizationTransactionNumber,
                        AuthorizationTransactionCode = o.AuthorizationTransactionCode,
                        AuthrorizationTransactionResult = o.AuthrorizationTransactionResult,
                        CustomerIpAddress = o.CustomerIpAddress,
                        CustomerDeviceInfo = o.CustomerDeviceInfo,
                        PaidDate = o.PaidDate,
                        PaidTime = o.PaidTime,
                        Id = o.Id,
                    },
                    OrderInvoiceNumber = o.OrderInvoiceNumber,
                    CurrencyName = o.CurrencyName,
                    PaymentTypeName = o.PaymentTypeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderPaymentInfoForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOrderPaymentInfoForViewDto> GetOrderPaymentInfoForView(long id)
        {
            var orderPaymentInfo = await _orderPaymentInfoRepository.GetAsync(id);

            var output = new GetOrderPaymentInfoForViewDto { OrderPaymentInfo = ObjectMapper.Map<OrderPaymentInfoDto>(orderPaymentInfo) };

            if (output.OrderPaymentInfo.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.OrderPaymentInfo.OrderId);
                output.OrderInvoiceNumber = _lookupOrder?.InvoiceNumber?.ToString();
            }

            if (output.OrderPaymentInfo.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.OrderPaymentInfo.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            if (output.OrderPaymentInfo.PaymentTypeId != null)
            {
                var _lookupPaymentType = await _lookup_paymentTypeRepository.FirstOrDefaultAsync((long)output.OrderPaymentInfo.PaymentTypeId);
                output.PaymentTypeName = _lookupPaymentType?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OrderPaymentInfos_Edit)]
        public async Task<GetOrderPaymentInfoForEditOutput> GetOrderPaymentInfoForEdit(EntityDto<long> input)
        {
            var orderPaymentInfo = await _orderPaymentInfoRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderPaymentInfoForEditOutput { OrderPaymentInfo = ObjectMapper.Map<CreateOrEditOrderPaymentInfoDto>(orderPaymentInfo) };

            if (output.OrderPaymentInfo.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.OrderPaymentInfo.OrderId);
                output.OrderInvoiceNumber = _lookupOrder?.InvoiceNumber?.ToString();
            }

            if (output.OrderPaymentInfo.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.OrderPaymentInfo.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            if (output.OrderPaymentInfo.PaymentTypeId != null)
            {
                var _lookupPaymentType = await _lookup_paymentTypeRepository.FirstOrDefaultAsync((long)output.OrderPaymentInfo.PaymentTypeId);
                output.PaymentTypeName = _lookupPaymentType?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrderPaymentInfoDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OrderPaymentInfos_Create)]
        protected virtual async Task Create(CreateOrEditOrderPaymentInfoDto input)
        {
            var orderPaymentInfo = ObjectMapper.Map<OrderPaymentInfo>(input);

            if (AbpSession.TenantId != null)
            {
                orderPaymentInfo.TenantId = (int?)AbpSession.TenantId;
            }

            await _orderPaymentInfoRepository.InsertAsync(orderPaymentInfo);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderPaymentInfos_Edit)]
        protected virtual async Task Update(CreateOrEditOrderPaymentInfoDto input)
        {
            var orderPaymentInfo = await _orderPaymentInfoRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, orderPaymentInfo);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderPaymentInfos_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _orderPaymentInfoRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOrderPaymentInfosToExcel(GetAllOrderPaymentInfosForExcelInput input)
        {

            var filteredOrderPaymentInfos = _orderPaymentInfoRepository.GetAll()
                        .Include(e => e.OrderFk)
                        .Include(e => e.CurrencyFk)
                        .Include(e => e.PaymentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BillingAddress.Contains(input.Filter) || e.BillingCity.Contains(input.Filter) || e.BillingState.Contains(input.Filter) || e.BillingZipCode.Contains(input.Filter) || e.MaskedCreditCardNumber.Contains(input.Filter) || e.CardName.Contains(input.Filter) || e.CardNumber.Contains(input.Filter) || e.CardCvv.Contains(input.Filter) || e.CardExpirationMonth.Contains(input.Filter) || e.CardExpirationYear.Contains(input.Filter) || e.AuthorizationTransactionNumber.Contains(input.Filter) || e.AuthorizationTransactionCode.Contains(input.Filter) || e.AuthrorizationTransactionResult.Contains(input.Filter) || e.CustomerIpAddress.Contains(input.Filter) || e.CustomerDeviceInfo.Contains(input.Filter) || e.PaidTime.Contains(input.Filter))
                        .WhereIf(input.PaymentSplitFilter.HasValue && input.PaymentSplitFilter > -1, e => (input.PaymentSplitFilter == 1 && e.PaymentSplit) || (input.PaymentSplitFilter == 0 && !e.PaymentSplit))
                        .WhereIf(input.MinDueAmountFilter != null, e => e.DueAmount >= input.MinDueAmountFilter)
                        .WhereIf(input.MaxDueAmountFilter != null, e => e.DueAmount <= input.MaxDueAmountFilter)
                        .WhereIf(input.MinPaySplitAmountFilter != null, e => e.PaySplitAmount >= input.MinPaySplitAmountFilter)
                        .WhereIf(input.MaxPaySplitAmountFilter != null, e => e.PaySplitAmount <= input.MaxPaySplitAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BillingAddressFilter), e => e.BillingAddress.Contains(input.BillingAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BillingCityFilter), e => e.BillingCity.Contains(input.BillingCityFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BillingStateFilter), e => e.BillingState.Contains(input.BillingStateFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BillingZipCodeFilter), e => e.BillingZipCode.Contains(input.BillingZipCodeFilter))
                        .WhereIf(input.SaveCreditCardNumberFilter.HasValue && input.SaveCreditCardNumberFilter > -1, e => (input.SaveCreditCardNumberFilter == 1 && e.SaveCreditCardNumber) || (input.SaveCreditCardNumberFilter == 0 && !e.SaveCreditCardNumber))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MaskedCreditCardNumberFilter), e => e.MaskedCreditCardNumber.Contains(input.MaskedCreditCardNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardNameFilter), e => e.CardName.Contains(input.CardNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardNumberFilter), e => e.CardNumber.Contains(input.CardNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardCvvFilter), e => e.CardCvv.Contains(input.CardCvvFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardExpirationMonthFilter), e => e.CardExpirationMonth.Contains(input.CardExpirationMonthFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CardExpirationYearFilter), e => e.CardExpirationYear.Contains(input.CardExpirationYearFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthorizationTransactionNumberFilter), e => e.AuthorizationTransactionNumber.Contains(input.AuthorizationTransactionNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthorizationTransactionCodeFilter), e => e.AuthorizationTransactionCode.Contains(input.AuthorizationTransactionCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuthrorizationTransactionResultFilter), e => e.AuthrorizationTransactionResult.Contains(input.AuthrorizationTransactionResultFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerIpAddressFilter), e => e.CustomerIpAddress.Contains(input.CustomerIpAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerDeviceInfoFilter), e => e.CustomerDeviceInfo.Contains(input.CustomerDeviceInfoFilter))
                        .WhereIf(input.MinPaidDateFilter != null, e => e.PaidDate >= input.MinPaidDateFilter)
                        .WhereIf(input.MaxPaidDateFilter != null, e => e.PaidDate <= input.MaxPaidDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaidTimeFilter), e => e.PaidTime.Contains(input.PaidTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderInvoiceNumberFilter), e => e.OrderFk != null && e.OrderFk.InvoiceNumber == input.OrderInvoiceNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentTypeNameFilter), e => e.PaymentTypeFk != null && e.PaymentTypeFk.Name == input.PaymentTypeNameFilter);

            var query = (from o in filteredOrderPaymentInfos
                         join o1 in _lookup_orderRepository.GetAll() on o.OrderId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_paymentTypeRepository.GetAll() on o.PaymentTypeId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetOrderPaymentInfoForViewDto()
                         {
                             OrderPaymentInfo = new OrderPaymentInfoDto
                             {
                                 PaymentSplit = o.PaymentSplit,
                                 DueAmount = o.DueAmount,
                                 PaySplitAmount = o.PaySplitAmount,
                                 BillingAddress = o.BillingAddress,
                                 BillingCity = o.BillingCity,
                                 BillingState = o.BillingState,
                                 BillingZipCode = o.BillingZipCode,
                                 SaveCreditCardNumber = o.SaveCreditCardNumber,
                                 MaskedCreditCardNumber = o.MaskedCreditCardNumber,
                                 CardName = o.CardName,
                                 CardNumber = o.CardNumber,
                                 CardCvv = o.CardCvv,
                                 CardExpirationMonth = o.CardExpirationMonth,
                                 CardExpirationYear = o.CardExpirationYear,
                                 AuthorizationTransactionNumber = o.AuthorizationTransactionNumber,
                                 AuthorizationTransactionCode = o.AuthorizationTransactionCode,
                                 AuthrorizationTransactionResult = o.AuthrorizationTransactionResult,
                                 CustomerIpAddress = o.CustomerIpAddress,
                                 CustomerDeviceInfo = o.CustomerDeviceInfo,
                                 PaidDate = o.PaidDate,
                                 PaidTime = o.PaidTime,
                                 Id = o.Id
                             },
                             OrderInvoiceNumber = s1 == null || s1.InvoiceNumber == null ? "" : s1.InvoiceNumber.ToString(),
                             CurrencyName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             PaymentTypeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var orderPaymentInfoListDtos = await query.ToListAsync();

            return _orderPaymentInfosExcelExporter.ExportToFile(orderPaymentInfoListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_OrderPaymentInfos)]
        public async Task<PagedResultDto<OrderPaymentInfoOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_orderRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.InvoiceNumber != null && e.InvoiceNumber.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var orderList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderPaymentInfoOrderLookupTableDto>();
            foreach (var order in orderList)
            {
                lookupTableDtoList.Add(new OrderPaymentInfoOrderLookupTableDto
                {
                    Id = order.Id,
                    DisplayName = order.InvoiceNumber?.ToString()
                });
            }

            return new PagedResultDto<OrderPaymentInfoOrderLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_OrderPaymentInfos)]
        public async Task<PagedResultDto<OrderPaymentInfoCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_currencyRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var currencyList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderPaymentInfoCurrencyLookupTableDto>();
            foreach (var currency in currencyList)
            {
                lookupTableDtoList.Add(new OrderPaymentInfoCurrencyLookupTableDto
                {
                    Id = currency.Id,
                    DisplayName = currency.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderPaymentInfoCurrencyLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_OrderPaymentInfos)]
        public async Task<PagedResultDto<OrderPaymentInfoPaymentTypeLookupTableDto>> GetAllPaymentTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_paymentTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var paymentTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderPaymentInfoPaymentTypeLookupTableDto>();
            foreach (var paymentType in paymentTypeList)
            {
                lookupTableDtoList.Add(new OrderPaymentInfoPaymentTypeLookupTableDto
                {
                    Id = paymentType.Id,
                    DisplayName = paymentType.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderPaymentInfoPaymentTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}