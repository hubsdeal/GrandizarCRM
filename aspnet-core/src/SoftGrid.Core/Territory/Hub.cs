using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Territory
{
    [Table("Hubs")]
    public class Hub : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(HubConsts.MaxNameLength, MinimumLength = HubConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual int? EstimatedPopulation { get; set; }

        public virtual bool HasParentHub { get; set; }

        public virtual long? ParentHubId { get; set; }

        public virtual decimal? Latitude { get; set; }

        public virtual decimal? Longitude { get; set; }

        public virtual bool Live { get; set; }

        [StringLength(HubConsts.MaxUrlLength, MinimumLength = HubConsts.MinUrlLength)]
        public virtual string Url { get; set; }

        [StringLength(HubConsts.MaxOfficeFullAddressLength, MinimumLength = HubConsts.MinOfficeFullAddressLength)]
        public virtual string OfficeFullAddress { get; set; }

        public virtual bool PartnerOrOwned { get; set; }

        public virtual Guid PictureId { get; set; }

        [StringLength(HubConsts.MaxPhoneLength, MinimumLength = HubConsts.MinPhoneLength)]
        public virtual string Phone { get; set; }

        public virtual string YearlyRevenue { get; set; }

        public virtual int? DisplaySequence { get; set; }

        public virtual long? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

        public virtual long? StateId { get; set; }

        [ForeignKey("StateId")]
        public State StateFk { get; set; }

        public virtual long? CityId { get; set; }

        [ForeignKey("CityId")]
        public City CityFk { get; set; }

        public virtual long? CountyId { get; set; }

        [ForeignKey("CountyId")]
        public County CountyFk { get; set; }

        public virtual long HubTypeId { get; set; }

        [ForeignKey("HubTypeId")]
        public HubType HubTypeFk { get; set; }

        public virtual long? CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public Currency CurrencyFk { get; set; }

    }
}