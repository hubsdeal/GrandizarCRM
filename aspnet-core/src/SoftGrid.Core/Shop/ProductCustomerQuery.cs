using SoftGrid.Shop;
using SoftGrid.CRM;
using SoftGrid.CRM;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("ProductCustomerQueries")]
    public class ProductCustomerQuery : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(ProductCustomerQueryConsts.MaxQuestionLength, MinimumLength = ProductCustomerQueryConsts.MinQuestionLength)]
        public virtual string Question { get; set; }

        public virtual string Answer { get; set; }

        public virtual DateTime? AnswerDate { get; set; }

        [StringLength(ProductCustomerQueryConsts.MaxAnswerTimeLength, MinimumLength = ProductCustomerQueryConsts.MinAnswerTimeLength)]
        public virtual string AnswerTime { get; set; }

        public virtual bool Publish { get; set; }

        public virtual long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual long? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

        public virtual long? EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee EmployeeFk { get; set; }

    }
}