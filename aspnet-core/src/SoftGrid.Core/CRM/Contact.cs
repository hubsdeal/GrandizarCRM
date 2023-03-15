using SoftGrid.Authorization.Users;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.CRM
{
    [Table("Contacts")]
    public class Contact : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ContactConsts.MaxFullNameLength, MinimumLength = ContactConsts.MinFullNameLength)]
        public virtual string FullName { get; set; }

        [Required]
        [StringLength(ContactConsts.MaxFirstNameLength, MinimumLength = ContactConsts.MinFirstNameLength)]
        public virtual string FirstName { get; set; }

        [StringLength(ContactConsts.MaxLastNameLength, MinimumLength = ContactConsts.MinLastNameLength)]
        public virtual string LastName { get; set; }

        [StringLength(ContactConsts.MaxFullAddressLength, MinimumLength = ContactConsts.MinFullAddressLength)]
        public virtual string FullAddress { get; set; }

        [StringLength(ContactConsts.MaxAddressLength, MinimumLength = ContactConsts.MinAddressLength)]
        public virtual string Address { get; set; }

        [StringLength(ContactConsts.MaxZipCodeLength, MinimumLength = ContactConsts.MinZipCodeLength)]
        public virtual string ZipCode { get; set; }

        [StringLength(ContactConsts.MaxCityLength, MinimumLength = ContactConsts.MinCityLength)]
        public virtual string City { get; set; }

        public virtual DateTime? DateOfBirth { get; set; }

        [StringLength(ContactConsts.MaxMobileLength, MinimumLength = ContactConsts.MinMobileLength)]
        public virtual string Mobile { get; set; }

        [StringLength(ContactConsts.MaxOfficePhoneLength, MinimumLength = ContactConsts.MinOfficePhoneLength)]
        public virtual string OfficePhone { get; set; }

        [StringLength(ContactConsts.MaxCountryCodeLength, MinimumLength = ContactConsts.MinCountryCodeLength)]
        public virtual string CountryCode { get; set; }

        //[RegularExpression(ContactConsts.PersonalEmailRegex)]
        [StringLength(ContactConsts.MaxPersonalEmailLength, MinimumLength = ContactConsts.MinPersonalEmailLength)]
        public virtual string PersonalEmail { get; set; }

        //[RegularExpression(ContactConsts.BusinessEmailRegex)]
        [StringLength(ContactConsts.MaxBusinessEmailLength, MinimumLength = ContactConsts.MinBusinessEmailLength)]
        public virtual string BusinessEmail { get; set; }

        [StringLength(ContactConsts.MaxJobTitleLength, MinimumLength = ContactConsts.MinJobTitleLength)]
        public virtual string JobTitle { get; set; }

        [StringLength(ContactConsts.MaxCompanyNameLength, MinimumLength = ContactConsts.MinCompanyNameLength)]
        public virtual string CompanyName { get; set; }

        public virtual string Profile { get; set; }

        public virtual string AiDataTag { get; set; }

        [StringLength(ContactConsts.MaxFacebookLength, MinimumLength = ContactConsts.MinFacebookLength)]
        public virtual string Facebook { get; set; }

        [StringLength(ContactConsts.MaxLinkedInLength, MinimumLength = ContactConsts.MinLinkedInLength)]
        public virtual string LinkedIn { get; set; }

        public virtual decimal? Latitude { get; set; }

        public virtual decimal? Longitude { get; set; }

        public virtual bool Referred { get; set; }

        [StringLength(ContactConsts.MaxFaxLength, MinimumLength = ContactConsts.MinFaxLength)]
        public virtual string Fax { get; set; }

        public virtual bool Verified { get; set; }

        public virtual int? Score { get; set; }

        public virtual long? ReferredByUserId { get; set; }

        [ForeignKey("ReferredByUserId")]
        public User ReferredByUserFk { get; set; }

        public virtual long? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

        public virtual long? StateId { get; set; }

        [ForeignKey("StateId")]
        public State StateFk { get; set; }

        public virtual long? MembershipTypeId { get; set; }

        [ForeignKey("MembershipTypeId")]
        public MembershipType MembershipTypeFk { get; set; }

    }
}