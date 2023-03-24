using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.Shop;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("StoreLocations")]
    public class StoreLocation : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(StoreLocationConsts.MaxLocationNameLength, MinimumLength = StoreLocationConsts.MinLocationNameLength)]
        public virtual string LocationName { get; set; }

        [StringLength(StoreLocationConsts.MaxFullAddressLength, MinimumLength = StoreLocationConsts.MinFullAddressLength)]
        public virtual string FullAddress { get; set; }

        public virtual decimal? Latitude { get; set; }

        public virtual decimal? Longitude { get; set; }

        [StringLength(StoreLocationConsts.MaxAddressLength, MinimumLength = StoreLocationConsts.MinAddressLength)]
        public virtual string Address { get; set; }

        [StringLength(StoreLocationConsts.MaxMobileLength, MinimumLength = StoreLocationConsts.MinMobileLength)]
        public virtual string Mobile { get; set; }

        [StringLength(StoreLocationConsts.MaxEmailLength, MinimumLength = StoreLocationConsts.MinEmailLength)]
        public virtual string Email { get; set; }

        [StringLength(StoreLocationConsts.MaxZipCodeLength, MinimumLength = StoreLocationConsts.MinZipCodeLength)]
        public virtual string ZipCode { get; set; }

        public virtual long? CityId { get; set; }

        [ForeignKey("CityId")]
        public City CityFk { get; set; }

        public virtual long? StateId { get; set; }

        [ForeignKey("StateId")]
        public State StateFk { get; set; }

        public virtual long? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

    }
}