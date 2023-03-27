using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("StoreRelevantStores")]
    public class StoreRelevantStore : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long RelevantStoreId { get; set; }

        public virtual long PrimaryStoreId { get; set; }

        [ForeignKey("PrimaryStoreId")]
        public Store PrimaryStoreFk { get; set; }

    }
}