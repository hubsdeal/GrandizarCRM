using SoftGrid.Shop;
using SoftGrid.CRM;
using SoftGrid.Shop;
using SoftGrid.Territory;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("ProductCustomerStats")]
    public class ProductCustomerStat : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual bool Clicked { get; set; }

        public virtual bool WishedOrFavorite { get; set; }

        public virtual bool Purchased { get; set; }

        public virtual int? PurchasedQuantity { get; set; }

        public virtual bool Shared { get; set; }

        public virtual string PageLink { get; set; }

        public virtual bool AppOrWeb { get; set; }

        public virtual string QuitFromLink { get; set; }

        public virtual long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual long? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

        public virtual long? StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

        public virtual long? HubId { get; set; }

        [ForeignKey("HubId")]
        public Hub HubFk { get; set; }

        public virtual long? SocialMediaId { get; set; }

        [ForeignKey("SocialMediaId")]
        public SocialMedia SocialMediaFk { get; set; }

    }
}