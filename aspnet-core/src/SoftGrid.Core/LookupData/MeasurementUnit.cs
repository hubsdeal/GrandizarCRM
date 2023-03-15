using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("MeasurementUnits")]
    public class MeasurementUnit : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(MeasurementUnitConsts.MaxNameLength, MinimumLength = MeasurementUnitConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}