using SoftGrid.Shop;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("StoreZipCodeMaps")]
    public class StoreZipCodeMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(StoreZipCodeMapConsts.MaxZipCodeLength, MinimumLength = StoreZipCodeMapConsts.MinZipCodeLength)]
        public virtual string ZipCode { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

        public virtual long? ZipCodeId { get; set; }

        [ForeignKey("ZipCodeId")]
        public ZipCode ZipCodeFk { get; set; }

    }
}