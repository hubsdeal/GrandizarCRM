using SoftGrid.DiscountManagement;
using SoftGrid.CRM;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.DiscountManagement
{
    [Table("DiscountCodeByCustomers")]
    public class DiscountCodeByCustomer : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long? DiscountCodeGeneratorId { get; set; }

        [ForeignKey("DiscountCodeGeneratorId")]
        public DiscountCodeGenerator DiscountCodeGeneratorFk { get; set; }

        public virtual long? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

    }
}