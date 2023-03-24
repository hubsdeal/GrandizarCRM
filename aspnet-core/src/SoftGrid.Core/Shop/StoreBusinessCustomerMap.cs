using SoftGrid.Shop;
using SoftGrid.CRM;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("StoreBusinessCustomerMaps")]
    public class StoreBusinessCustomerMap : CreationAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual bool PaidCustomer { get; set; }

        public virtual double? LifeTimeSalesAmount { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

        public virtual long BusinessId { get; set; }

        [ForeignKey("BusinessId")]
        public Business BusinessFk { get; set; }

    }
}