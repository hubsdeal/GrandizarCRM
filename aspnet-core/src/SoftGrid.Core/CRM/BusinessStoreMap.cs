using SoftGrid.CRM;
using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.CRM
{
    [Table("BusinessStoreMaps")]
    public class BusinessStoreMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long BusinessId { get; set; }

        [ForeignKey("BusinessId")]
        public Business BusinessFk { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

    }
}