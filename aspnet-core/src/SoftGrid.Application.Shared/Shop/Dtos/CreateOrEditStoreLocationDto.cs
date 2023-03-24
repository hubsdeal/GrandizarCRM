using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreLocationDto : EntityDto<long?>
    {

        [Required]
        [StringLength(StoreLocationConsts.MaxLocationNameLength, MinimumLength = StoreLocationConsts.MinLocationNameLength)]
        public string LocationName { get; set; }

        [StringLength(StoreLocationConsts.MaxFullAddressLength, MinimumLength = StoreLocationConsts.MinFullAddressLength)]
        public string FullAddress { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        [StringLength(StoreLocationConsts.MaxAddressLength, MinimumLength = StoreLocationConsts.MinAddressLength)]
        public string Address { get; set; }

        [StringLength(StoreLocationConsts.MaxMobileLength, MinimumLength = StoreLocationConsts.MinMobileLength)]
        public string Mobile { get; set; }

        [StringLength(StoreLocationConsts.MaxEmailLength, MinimumLength = StoreLocationConsts.MinEmailLength)]
        public string Email { get; set; }

        [StringLength(StoreLocationConsts.MaxZipCodeLength, MinimumLength = StoreLocationConsts.MinZipCodeLength)]
        public string ZipCode { get; set; }

        public long? CityId { get; set; }

        public long? StateId { get; set; }

        public long? CountryId { get; set; }

        public long StoreId { get; set; }

    }
}