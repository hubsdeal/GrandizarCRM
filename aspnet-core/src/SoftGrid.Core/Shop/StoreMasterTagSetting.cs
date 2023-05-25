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
    [Table("StoreMasterTagSettings")]
    public class StoreMasterTagSetting : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(StoreMasterTagSettingConsts.MaxCustomTagTitleLength, MinimumLength = StoreMasterTagSettingConsts.MinCustomTagTitleLength)]
        public virtual string CustomTagTitle { get; set; }

        [StringLength(StoreMasterTagSettingConsts.MaxCustomTagChatQuestionLength, MinimumLength = StoreMasterTagSettingConsts.MinCustomTagChatQuestionLength)]
        public virtual string CustomTagChatQuestion { get; set; }

        public virtual bool DisplayPublic { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual AnswerType AnswerTypeId { get; set; }

        public virtual long StoreTagSettingCategoryId { get; set; }

        [ForeignKey("StoreTagSettingCategoryId")]
        public StoreTagSettingCategory StoreTagSettingCategoryFk { get; set; }

        public virtual long? MasterTagCategoryId { get; set; }

        [ForeignKey("MasterTagCategoryId")]
        public MasterTagCategory MasterTagCategoryFk { get; set; }

    }
}