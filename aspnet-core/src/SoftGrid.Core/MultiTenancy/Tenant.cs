using System;
using System.ComponentModel.DataAnnotations;
using Abp.MultiTenancy;
using Abp.Timing;
using SoftGrid.Authorization.Users;
using SoftGrid.Editions;
using SoftGrid.MultiTenancy.Payments;

namespace SoftGrid.MultiTenancy
{
    /// <summary>
    /// Represents a Tenant in the system.
    /// A tenant is a isolated customer for the application
    /// which has it's own users, roles and other application entities.
    /// </summary>
    public class Tenant : AbpTenant<User>
    {
        public const int MaxLogoMimeTypeLength = 64;

        //Can add application specific tenant properties here

        public DateTime? SubscriptionEndDateUtc { get; set; }

        public bool IsInTrialPeriod { get; set; }

        public virtual Guid? CustomCssId { get; set; }

        public virtual Guid? DarkLogoId { get; set; }

        [MaxLength(MaxLogoMimeTypeLength)]
        public virtual string DarkLogoFileType { get; set; }
        
        public virtual Guid? LightLogoId { get; set; }

        [MaxLength(MaxLogoMimeTypeLength)]
        public virtual string LightLogoFileType { get; set; }

        public SubscriptionPaymentType SubscriptionPaymentType { get; set; }

        protected Tenant()
        {

        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {

        }

        public virtual bool HasLogo()
        {
            return (DarkLogoId != null && DarkLogoFileType != null) ||
                   (LightLogoId != null && LightLogoFileType != null);
        }
        
        public virtual bool HasDarkLogo()
        {
            return DarkLogoId != null && DarkLogoFileType != null;
        }     
        
        public virtual bool HasLightLogo()
        {
            return LightLogoId != null && LightLogoFileType != null;
        }

        public void ClearLogo()
        {
            DarkLogoId = null;
            DarkLogoFileType = null;
        }

        public void UpdateSubscriptionDateForPayment(PaymentPeriodType paymentPeriodType, EditionPaymentType editionPaymentType)
        {
            switch (editionPaymentType)
            {
                case EditionPaymentType.NewRegistration:
                case EditionPaymentType.BuyNow:
                    {
                        SubscriptionEndDateUtc = Clock.Now.ToUniversalTime().AddDays((int)paymentPeriodType);
                        break;
                    }
                case EditionPaymentType.Extend:
                    ExtendSubscriptionDate(paymentPeriodType);
                    break;
                case EditionPaymentType.Upgrade:
                    if (HasUnlimitedTimeSubscription())
                    {
                        SubscriptionEndDateUtc = Clock.Now.ToUniversalTime().AddDays((int)paymentPeriodType);
                    }
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void ExtendSubscriptionDate(PaymentPeriodType paymentPeriodType)
        {
            if (SubscriptionEndDateUtc == null)
            {
                throw new InvalidOperationException("Can not extend subscription date while it's null!");
            }

            if (IsSubscriptionEnded())
            {
                SubscriptionEndDateUtc = Clock.Now.ToUniversalTime();
            }

            SubscriptionEndDateUtc = SubscriptionEndDateUtc.Value.AddDays((int)paymentPeriodType);
        }

        private bool IsSubscriptionEnded()
        {
            return SubscriptionEndDateUtc < Clock.Now.ToUniversalTime();
        }

        public int CalculateRemainingHoursCount()
        {
            return SubscriptionEndDateUtc != null
                ? (int)(SubscriptionEndDateUtc.Value - Clock.Now.ToUniversalTime()).TotalHours //converting it to int is not a problem since max value ((DateTime.MaxValue - DateTime.MinValue).TotalHours = 87649416) is in range of integer.
                : 0;
        }

        public bool HasUnlimitedTimeSubscription()
        {
            return SubscriptionEndDateUtc == null;
        }
    }
}