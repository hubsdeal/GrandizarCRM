using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetAllOrderPaymentInfosForExcelInput
    {
        public string Filter { get; set; }

        public int? PaymentSplitFilter { get; set; }

        public double? MaxDueAmountFilter { get; set; }
        public double? MinDueAmountFilter { get; set; }

        public double? MaxPaySplitAmountFilter { get; set; }
        public double? MinPaySplitAmountFilter { get; set; }

        public string BillingAddressFilter { get; set; }

        public string BillingCityFilter { get; set; }

        public string BillingStateFilter { get; set; }

        public string BillingZipCodeFilter { get; set; }

        public int? SaveCreditCardNumberFilter { get; set; }

        public string MaskedCreditCardNumberFilter { get; set; }

        public string CardNameFilter { get; set; }

        public string CardNumberFilter { get; set; }

        public string CardCvvFilter { get; set; }

        public string CardExpirationMonthFilter { get; set; }

        public string CardExpirationYearFilter { get; set; }

        public string AuthorizationTransactionNumberFilter { get; set; }

        public string AuthorizationTransactionCodeFilter { get; set; }

        public string AuthrorizationTransactionResultFilter { get; set; }

        public string CustomerIpAddressFilter { get; set; }

        public string CustomerDeviceInfoFilter { get; set; }

        public DateTime? MaxPaidDateFilter { get; set; }
        public DateTime? MinPaidDateFilter { get; set; }

        public string PaidTimeFilter { get; set; }

        public string OrderInvoiceNumberFilter { get; set; }

        public string CurrencyNameFilter { get; set; }

        public string PaymentTypeNameFilter { get; set; }

    }
}