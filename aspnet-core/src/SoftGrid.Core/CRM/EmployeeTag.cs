using SoftGrid.CRM;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.CRM
{
    [Table("EmployeeTags")]
    public class EmployeeTag : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(EmployeeTagConsts.MaxCustomTagLength, MinimumLength = EmployeeTagConsts.MinCustomTagLength)]
        public virtual string CustomTag { get; set; }

        [StringLength(EmployeeTagConsts.MaxTagValueLength, MinimumLength = EmployeeTagConsts.MinTagValueLength)]
        public virtual string TagValue { get; set; }

        public virtual bool Verified { get; set; }

        public virtual int? Sequence { get; set; }

        public virtual long EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee EmployeeFk { get; set; }

        public virtual long? MasterTagCategoryId { get; set; }

        [ForeignKey("MasterTagCategoryId")]
        public MasterTagCategory MasterTagCategoryFk { get; set; }

        public virtual long? MasterTagId { get; set; }

        [ForeignKey("MasterTagId")]
        public MasterTag MasterTagFk { get; set; }

    }
}