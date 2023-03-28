using SoftGrid.Territory;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Territory
{
    [Table("HubZipCodeMaps")]
    public class HubZipCodeMap : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(HubZipCodeMapConsts.MaxCityNameLength, MinimumLength = HubZipCodeMapConsts.MinCityNameLength)]
        public virtual string CityName { get; set; }

        [StringLength(HubZipCodeMapConsts.MaxZipCodeLength, MinimumLength = HubZipCodeMapConsts.MinZipCodeLength)]
        public virtual string ZipCode { get; set; }

        public virtual long HubId { get; set; }

        [ForeignKey("HubId")]
        public Hub HubFk { get; set; }

        public virtual long? CityId { get; set; }

        [ForeignKey("CityId")]
        public City CityFk { get; set; }

        public virtual long? ZipCodeId { get; set; }

        [ForeignKey("ZipCodeId")]
        public ZipCode ZipCodeFk { get; set; }

    }
}