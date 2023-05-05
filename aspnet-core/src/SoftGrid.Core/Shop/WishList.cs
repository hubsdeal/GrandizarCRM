using SoftGrid.CRM;
using SoftGrid.Shop;
using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("WishLists")]
    public class WishList : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual DateTime? Date { get; set; }

        public virtual long ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

        public virtual long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual long? StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

    }
}