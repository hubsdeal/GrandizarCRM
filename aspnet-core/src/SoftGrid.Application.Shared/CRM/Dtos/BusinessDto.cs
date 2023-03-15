using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.CRM.Dtos
{
    public class BusinessDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string TradeName { get; set; }

        public string Description { get; set; }

        public string CustomId { get; set; }

        public string YearOfEstablishment { get; set; }

        public string LocationTitle { get; set; }

        public string FullAddress { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public string EinTaxId { get; set; }

        public string Industry { get; set; }

        public string InternalRemarks { get; set; }

        public bool Verified { get; set; }

        public string Facebook { get; set; }

        public string LinkedIn { get; set; }

        public long? CountryId { get; set; }

        public long? StateId { get; set; }

        public long? CityId { get; set; }

        public long? LogoMediaLibraryId { get; set; }

    }
}