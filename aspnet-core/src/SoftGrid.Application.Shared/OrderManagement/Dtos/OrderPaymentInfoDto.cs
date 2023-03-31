using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.OrderManagement.Dtos
{
    public class OrderPaymentInfoDto : EntityDto<long>
    {
        public bool PaymentSplit { get; set; }

        public double? DueAmount { get; set; }

        public double? PaySplitAmount { get; set; }

        public string BillingAddress { get; set; }

        public string BillingCity { get; set; }

        public string BillingState { get; set; }

        public string BillingZipCode { get; set; }

        public bool SaveCreditCardNumber { get; set; }

        public string MaskedCreditCardNumber { get; set; }

        public string CardName { get; set; }

        public string CardNumber { get; set; }

        public string CardCvv { get; set; }

        public string CardExpirationMonth { get; set; }

        public string CardExpirationYear { get; set; }

        public string AuthorizationTransactionNumber { get; set; }

        public string AuthorizationTransactionCode { get; set; }

        public string AuthrorizationTransactionResult { get; set; }

        public string CustomerIpAddress { get; set; }

        public string CustomerDeviceInfo { get; set; }

        public DateTime? PaidDate { get; set; }

        public string PaidTime { get; set; }

        public long? OrderId { get; set; }

        public long? CurrencyId { get; set; }

        public long? PaymentTypeId { get; set; }

    }
}