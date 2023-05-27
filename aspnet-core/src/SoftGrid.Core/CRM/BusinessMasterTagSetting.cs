using SoftGrid.Shop.Enums;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.CRM
{
    [Table("BusinessMasterTagSettings")]
    public class BusinessMasterTagSetting : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual bool DisplayPublic { get; set; }

        [StringLength(BusinessMasterTagSettingConsts.MaxCustomTagTitleLength, MinimumLength = BusinessMasterTagSettingConsts.MinCustomTagTitleLength)]
        public virtual string CustomTagTitle { get; set; }

        [StringLength(BusinessMasterTagSettingConsts.MaxCustomTagChatQuestionLength, MinimumLength = BusinessMasterTagSettingConsts.MinCustomTagChatQuestionLength)]
        public virtual string CustomTagChatQuestion { get; set; }

        public virtual AnswerType AnswerTypeId { get; set; }

        public virtual long? MasterTagCategoryId { get; set; }

        [ForeignKey("MasterTagCategoryId")]
        public MasterTagCategory MasterTagCategoryFk { get; set; }

        public virtual long BusinessTypeId { get; set; }

        [ForeignKey("BusinessTypeId")]
        public MasterTag BusinessTypeFk { get; set; }

    }
}