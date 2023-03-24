using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreLocationDto : EntityDto<long>
    {
        public string LocationName { get; set; }

        public string FullAddress { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string Address { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string ZipCode { get; set; }

        public long? CityId { get; set; }

        public long? StateId { get; set; }

        public long? CountryId { get; set; }

        public long StoreId { get; set; }

    }
}