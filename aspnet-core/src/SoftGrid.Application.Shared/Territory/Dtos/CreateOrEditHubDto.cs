using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class CreateOrEditHubDto : EntityDto<long?>
    {

        [Required]
        [StringLength(HubConsts.MaxNameLength, MinimumLength = HubConsts.MinNameLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? EstimatedPopulation { get; set; }

        public bool HasParentHub { get; set; }

        public long? ParentHubId { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public bool Live { get; set; }

        [StringLength(HubConsts.MaxUrlLength, MinimumLength = HubConsts.MinUrlLength)]
        public string Url { get; set; }

        [StringLength(HubConsts.MaxOfficeFullAddressLength, MinimumLength = HubConsts.MinOfficeFullAddressLength)]
        public string OfficeFullAddress { get; set; }

        public bool PartnerOrOwned { get; set; }

        public Guid PictureId { get; set; }

        [StringLength(HubConsts.MaxPhoneLength, MinimumLength = HubConsts.MinPhoneLength)]
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