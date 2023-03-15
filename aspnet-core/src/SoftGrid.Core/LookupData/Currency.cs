using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("Currencies")]
    public class Currency : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(CurrencyConsts.MaxNameLength, MinimumLength = CurrencyConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(CurrencyConsts.MaxTickerLength, MinimumLength = CurrencyConsts.MinTickerLength)]
        public virtual string Ticker { get; set; }

        [StringLength(CurrencyConsts.MaxIconLength, MinimumLength = CurrencyConsts.MinIconLength)]
        public virtual string Icon { get; set; }

    }
}