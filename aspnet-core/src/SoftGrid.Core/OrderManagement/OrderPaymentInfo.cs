using SoftGrid.OrderManagement;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.OrderManagement
{
    [Table("OrderPaymentInfos")]
    public class OrderPaymentInfo : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual bool PaymentSplit { get; set; }

        public virtual double? DueAmount { get; set; }

        public virtual double? PaySplitAmount { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxBillingAddressLength, MinimumLength = OrderPaymentInfoConsts.MinBillingAddressLength)]
        public virtual string BillingAddress { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxBillingCityLength, MinimumLength = OrderPaymentInfoConsts.MinBillingCityLength)]
        public virtual string BillingCity { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxBillingStateLength, MinimumLength = OrderPaymentInfoConsts.MinBillingStateLength)]
        public virtual string BillingState { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxBillingZipCodeLength, MinimumLength = OrderPaymentInfoConsts.MinBillingZipCodeLength)]
        public virtual string BillingZipCode { get; set; }

        public virtual bool SaveCreditCardNumber { get; set; }

        public virtual string MaskedCreditCardNumber { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxCardNameLength, MinimumLength = OrderPaymentInfoConsts.MinCardNameLength)]
        public virtual string CardName { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxCardNumberLength, MinimumLength = OrderPaymentInfoConsts.MinCardNumberLength)]
        public virtual string CardNumber { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxCardCvvLength, MinimumLength = OrderPaymentInfoConsts.MinCardCvvLength)]
        public virtual string CardCvv { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxCardExpirationMonthLength, MinimumLength = OrderPaymentInfoConsts.MinCardExpirationMonthLength)]
        public virtual string CardExpirationMonth { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxCardExpirationYearLength, MinimumLength = OrderPaymentInfoConsts.MinCardExpirationYearLength)]
        public virtual string CardExpirationYear { get; set; }

        public virtual string AuthorizationTransactionNumber { get; set; }

        public virtual string AuthorizationTransactionCode { get; set; }

        public virtual string AuthrorizationTransactionResult { get; set; }

        public virtual string CustomerIpAddress { get; set; }

        public virtual string CustomerDeviceInfo { get; set; }

        public virtual DateTime? PaidDate { get; set; }

        [StringLength(OrderPaymentInfoConsts.MaxPaidTimeLength, MinimumLength = OrderPaymentInfoConsts.MinPaidTimeLength)]
        public virtual string PaidTime { get; set; }

        public virtual long? OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order OrderFk { get; set; }

        public virtual long? CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public Currency CurrencyFk { get; set; }

        public virtual long? PaymentTypeId { get; set; }

        [ForeignKey("PaymentTypeId")]
        public PaymentType PaymentTypeFk { get; set; }

    }
}