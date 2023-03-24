using SoftGrid.Shop;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("StoreMedias")]
    public class StoreMedia : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

        public virtual long MediaLibraryId { get; set; }

        [ForeignKey("MediaLibraryId")]
        public MediaLibrary MediaLibraryFk { get; set; }

    }
}