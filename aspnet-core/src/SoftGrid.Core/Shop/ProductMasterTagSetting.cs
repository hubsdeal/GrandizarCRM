using SoftGrid.Shop.Enums;
using SoftGrid.Shop;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("ProductMasterTagSettings")]
    public class ProductMasterTagSetting : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int? DisplaySequence { get; set; }

        [Required]
        [StringLength(ProductMasterTagSettingConsts.MaxCustomTagTitleLength, MinimumLength = ProductMasterTagSettingConsts.MinCustomTagTitleLength)]
        public virtual string CustomTagTitle { get; set; }

        [StringLength(ProductMasterTagSettingConsts.MaxCustomTagChatQuestionLength, MinimumLength = ProductMasterTagSettingConsts.MinCustomTagChatQuestionLength)]
        public virtual string CustomTagChatQuestion { get; set; }

        public virtual bool DisplayPublic { get; set; }

        public virtual AnswerType AnswerTypeId { get; set; }

        public virtual long ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategoryFk { get; set; }

        public virtual long? MasterTagCategoryId { get; set; }

        [ForeignKey("MasterTagCategoryId")]
        public MasterTagCategory MasterTagCategoryFk { get; set; }

    }
}