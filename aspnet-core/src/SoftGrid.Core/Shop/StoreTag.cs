using SoftGrid.Shop;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("StoreTags")]
    public class StoreTag : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(StoreTagConsts.MaxCustomTagLength, MinimumLength = StoreTagConsts.MinCustomTagLength)]
        public virtual string CustomTag { get; set; }

        [StringLength(StoreTagConsts.MaxTagValueLength, MinimumLength = StoreTagConsts.MinTagValueLength)]
        public virtual string TagValue { get; set; }

        public virtual bool Verified { get; set; }

        public virtual int? Sequence { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

        public virtual long? MasterTagCategoryId { get; set; }

        [ForeignKey("MasterTagCategoryId")]
        public MasterTagCategory MasterTagCategoryFk { get; set; }

        public virtual long? MasterTagId { get; set; }

        [ForeignKey("MasterTagId")]
        public MasterTag MasterTagFk { get; set; }

        public virtual bool? Published { get; set; }

    }
}