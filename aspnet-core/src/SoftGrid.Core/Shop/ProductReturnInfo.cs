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
    [Table("ProductReturnInfos")]
    public class ProductReturnInfo : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string CustomerNote { get; set; }

        public virtual string AdminNote { get; set; }

        public virtual long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual long? ReturnTypeId { get; set; }

        [ForeignKey("ReturnTypeId")]
        public ReturnType ReturnTypeFk { get; set; }

        public virtual long? ReturnStatusId { get; set; }

        [ForeignKey("ReturnStatusId")]
        public ReturnStatus ReturnStatusFk { get; set; }

    }
}