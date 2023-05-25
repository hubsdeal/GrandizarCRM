using SoftGrid.Shop.Enums;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.JobManagement
{
    [Table("JobMasterTagSettings")]
    public class JobMasterTagSetting : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual bool DisplayPublic { get; set; }

        public virtual AnswerType AnswerTypeId { get; set; }

        [StringLength(JobMasterTagSettingConsts.MaxCustomTagTitleLength, MinimumLength = JobMasterTagSettingConsts.MinCustomTagTitleLength)]
        public virtual string CustomTagTitle { get; set; }

        [StringLength(JobMasterTagSettingConsts.MaxCustomTagChatQuestionLength, MinimumLength = JobMasterTagSettingConsts.MinCustomTagChatQuestionLength)]
        public virtual string CustomTagChatQuestion { get; set; }

        public virtual long JobCategoryId { get; set; }

        [ForeignKey("JobCategoryId")]
        public MasterTag JobCategoryFk { get; set; }

        public virtual long? MasterTagCategoryId { get; set; }

        [ForeignKey("MasterTagCategoryId")]
        public MasterTagCategory MasterTagCategoryFk { get; set; }

    }
}