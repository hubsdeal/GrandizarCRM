using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class CreateOrEditOrderPaymentInfoDto : EntityDto<long?>
    {

        public bool PaymentSplit { get; set; }

        public double? DueAmount { get; set; }

        public double? PaySplitAmount { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxBillingAddressLength, MinimumLength = OrderPaymentInfoConsts.MinBillingAddressLength)]
        public string BillingAddress { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxBillingCityLength, MinimumLength = OrderPaymentInfoConsts.MinBillingCityLength)]
        public string BillingCity { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxBillingStateLength, MinimumLength = OrderPaymentInfoConsts.MinBillingStateLength)]
        public string BillingState { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxBillingZipCodeLength, MinimumLength = OrderPaymentInfoConsts.MinBillingZipCodeLength)]
        public string BillingZipCode { get; set; }

        public bool SaveCreditCardNumber { get; set; }

        public string MaskedCreditCardNumber { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxCardNameLength, MinimumLength = OrderPaymentInfoConsts.MinCardNameLength)]
        public string CardName { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxCardNumberLength, MinimumLength = OrderPaymentInfoConsts.MinCardNumberLength)]
        public string CardNumber { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxCardCvvLength, MinimumLength = OrderPaymentInfoConsts.MinCardCvvLength)]
        public string CardCvv { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxCardExpirationMonthLength, MinimumLength = OrderPaymentInfoConsts.MinCardExpirationMonthLength)]
        public string CardExpirationMonth { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxCardExpirationYearLength, MinimumLength = OrderPaymentInfoConsts.MinCardExpirationYearLength)]
        public string CardExpirationYear { get; set; }

        public string AuthorizationTransactionNumber { get; set; }

        public string AuthorizationTransactionCode { get; set; }

        public string AuthrorizationTransactionResult { get; set; }

        public string CustomerIpAddress { get; set; }

        public string CustomerDeviceInfo { get; set; }

        public DateTime? PaidDate { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxPaidTimeLength, MinimumLength = OrderPaymentInfoConsts.MinPaidTimeLength)]
        public string PaidTime { get; set; }

        public long? OrderId { get; set; }

        public long? CurrencyId { get; set; }

        public long? PaymentTypeId { get; set; }

    }
}