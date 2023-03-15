using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("Countries")]
    public class Country : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(CountryConsts.MaxNameLength, MinimumLength = CountryConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(CountryConsts.MaxTickerLength, MinimumLength = CountryConsts.MinTickerLength)]
        public virtual string Ticker { get; set; }

        [StringLength(CountryConsts.MaxFlagIconLength, MinimumLength = CountryConsts.MinFlagIconLength)]
        public virtual string FlagIcon { get; set; }

        [StringLength(CountryConsts.MaxPhoneCodeLength, MinimumLength = CountryConsts.MinPhoneCodeLength)]
        public virtual string PhoneCode { get; set; }

    }
}