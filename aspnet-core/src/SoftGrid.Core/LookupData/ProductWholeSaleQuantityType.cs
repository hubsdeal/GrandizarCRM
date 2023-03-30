using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("ProductWholeSaleQuantityTypes")]
    public class ProductWholeSaleQuantityType : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ProductWholeSaleQuantityTypeConsts.MaxNameLength, MinimumLength = ProductWholeSaleQuantityTypeConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual int? MinQty { get; set; }

        public virtual int? MaxQty { get; set; }

    }
}