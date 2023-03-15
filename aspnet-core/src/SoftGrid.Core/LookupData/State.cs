using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("States")]
    public class State : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(StateConsts.MaxNameLength, MinimumLength = StateConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(StateConsts.MaxTickerLength, MinimumLength = StateConsts.MinTickerLength)]
        public virtual string Ticker { get; set; }

        public virtual long CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

    }
}