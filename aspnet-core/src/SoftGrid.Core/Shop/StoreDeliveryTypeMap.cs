using SoftGrid.LookupData;
using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("StoreDeliveryTypeMaps")]
    public class StoreDeliveryTypeMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int? DeliveryTypeId { get; set; }

        [ForeignKey("DeliveryTypeId")]
        public DeliveryType DeliveryTypeFk { get; set; }

        public virtual long? StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

    }
}