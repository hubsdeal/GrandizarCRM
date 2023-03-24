using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("StoreNotes")]
    public class StoreNote : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Notes { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

    }
}