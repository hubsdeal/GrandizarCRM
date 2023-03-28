using SoftGrid.Territory;
using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Territory
{
    [Table("HubStores")]
    public class HubStore : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual bool Published { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual long HubId { get; set; }

        [ForeignKey("HubId")]
        public Hub HubFk { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

    }
}