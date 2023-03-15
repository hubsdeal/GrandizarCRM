using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Territory.Dtos
{
    public class HubDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int? EstimatedPopulation { get; set; }

        public bool HasParentHub { get; set; }

        public long? ParentHubId { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public bool Live { get; set; }

        public string Url { get; set; }

        public string OfficeFullAddress { get; set; }

        public bool PartnerOrOwned { get; set; }

        public Guid PictureId { get; set; }

        public string Phone { get; set; }

        public string YearlyRevenue { get; set; }

        public int? DisplaySequence { get; set; }

        public long? CountryId { get; set; }

        public long? StateId { get; set; }

        public long? CityId { get; set; }

        public long? CountyId { get; set; }

        public long HubTypeId { get; set; }

        public long? CurrencyId { get; set; }

    }
}