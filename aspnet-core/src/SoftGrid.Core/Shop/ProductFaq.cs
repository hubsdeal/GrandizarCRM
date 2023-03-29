using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("ProductFaqs")]
    public class ProductFaq : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ProductFaqConsts.MaxQuestionLength, MinimumLength = ProductFaqConsts.MinQuestionLength)]
        public virtual string Question { get; set; }

        public virtual string Answer { get; set; }

        public virtual bool Template { get; set; }

        public virtual bool Publish { get; set; }

        public virtual long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

    }
}