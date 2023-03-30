using SoftGrid.Shop;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("ProductPackages")]
    public class ProductPackage : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual long? PackageProductId { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual double? Price { get; set; }

        public virtual int? Quantity { get; set; }

        public virtual long PrimaryProductId { get; set; }

        [ForeignKey("PrimaryProductId")]
        public Product PrimaryProductFk { get; set; }

        public virtual long? MediaLibraryId { get; set; }

        [ForeignKey("MediaLibraryId")]
        public MediaLibrary MediaLibraryFk { get; set; }

    }
}