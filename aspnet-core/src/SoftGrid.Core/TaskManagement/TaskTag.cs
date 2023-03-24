using SoftGrid.TaskManagement;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.TaskManagement
{
    [Table("TaskTags")]
    public class TaskTag : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(TaskTagConsts.MaxCustomTagLength, MinimumLength = TaskTagConsts.MinCustomTagLength)]
        public virtual string CustomTag { get; set; }

        [StringLength(TaskTagConsts.MaxTagValueLength, MinimumLength = TaskTagConsts.MinTagValueLength)]
        public virtual string TagValue { get; set; }

        public virtual bool Verfied { get; set; }

        public virtual int? Sequence { get; set; }

        public virtual long TaskEventId { get; set; }

        [ForeignKey("TaskEventId")]
        public TaskEvent TaskEventFk { get; set; }

        public virtual long? MasterTagCategoryId { get; set; }

        [ForeignKey("MasterTagCategoryId")]
        public MasterTagCategory MasterTagCategoryFk { get; set; }

        public virtual long? MasterTagId { get; set; }

        [ForeignKey("MasterTagId")]
        public MasterTag MasterTagFk { get; set; }

    }
}