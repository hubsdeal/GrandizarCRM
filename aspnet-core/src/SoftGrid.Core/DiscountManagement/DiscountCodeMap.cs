using SoftGrid.DiscountManagement;
using SoftGrid.Shop;
using SoftGrid.Shop;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.DiscountManagement
{
    [Table("DiscountCodeMaps")]
    public class DiscountCodeMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long? DiscountCodeGeneratorId { get; set; }

        [ForeignKey("DiscountCodeGeneratorId")]
        public DiscountCodeGenerator DiscountCodeGeneratorFk { get; set; }

        public virtual long? StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

        public virtual long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual long? MembershipTypeId { get; set; }

        [ForeignKey("MembershipTypeId")]
        public MembershipType MembershipTypeFk { get; set; }

    }
}