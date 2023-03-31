using SoftGrid.CRM;
using SoftGrid.Shop;
using SoftGrid.Shop;
using SoftGrid.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("ProductOwnerPublicContactInfos")]
    public class ProductOwnerPublicContactInfo : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(ProductOwnerPublicContactInfoConsts.MaxNameLength, MinimumLength = ProductOwnerPublicContactInfoConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(ProductOwnerPublicContactInfoConsts.MaxMobileLength, MinimumLength = ProductOwnerPublicContactInfoConsts.MinMobileLength)]
        public virtual string Mobile { get; set; }

        [StringLength(ProductOwnerPublicContactInfoConsts.MaxEmailLength, MinimumLength = ProductOwnerPublicContactInfoConsts.MinEmailLength)]
        public virtual string Email { get; set; }

        [StringLength(ProductOwnerPublicContactInfoConsts.MaxShortBioLength, MinimumLength = ProductOwnerPublicContactInfoConsts.MinShortBioLength)]
        public virtual string ShortBio { get; set; }

        public virtual bool Publish { get; set; }

        public virtual Guid PhotoId { get; set; }

        public virtual long? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

        public virtual long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual long? StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

    }
}