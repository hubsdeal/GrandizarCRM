using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditContactDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ContactConsts.MaxFullNameLength, MinimumLength = ContactConsts.MinFullNameLength)]
        public string FullName { get; set; }

        [Required]
        [StringLength(ContactConsts.MaxFirstNameLength, MinimumLength = ContactConsts.MinFirstNameLength)]
        public string FirstName { get; set; }

        [StringLength(ContactConsts.MaxLastNameLength, MinimumLength = ContactConsts.MinLastNameLength)]
        public string LastName { get; set; }

        [StringLength(ContactConsts.MaxFullAddressLength, MinimumLength = ContactConsts.MinFullAddressLength)]
        public string FullAddress { get; set; }

        [StringLength(ContactConsts.MaxAddressLength, MinimumLength = ContactConsts.MinAddressLength)]
        public string Address { get; set; }

        [StringLength(ContactConsts.MaxZipCodeLength, MinimumLength = ContactConsts.MinZipCodeLength)]
        public string ZipCode { get; set; }

        [StringLength(ContactConsts.MaxCityLength, MinimumLength = ContactConsts.MinCityLength)]
        public string City { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(ContactConsts.MaxMobileLength, MinimumLength = ContactConsts.MinMobileLength)]
        public string Mobile { get; set; }

        [StringLength(ContactConsts.MaxOfficePhoneLength, MinimumLength = ContactConsts.MinOfficePhoneLength)]
        public string OfficePhone { get; set; }

        [StringLength(ContactConsts.MaxCountryCodeLength, MinimumLength = ContactConsts.MinCountryCodeLength)]
        public string CountryCode { get; set; }

        //[RegularExpression(ContactConsts.PersonalEmailRegex)]
        [StringLength(ContactConsts.MaxPersonalEmailLength, MinimumLength = ContactConsts.MinPersonalEmailLength)]
        public string PersonalEmail { get; set; }

       // [RegularExpression(ContactConsts.BusinessEmailRegex)]
        [StringLength(ContactConsts.MaxBusinessEmailLength, MinimumLength = ContactConsts.MinBusinessEmailLength)]
        public string BusinessEmail { get; set; }

        [StringLength(ContactConsts.MaxJobTitleLength, MinimumLength = ContactConsts.MinJobTitleLength)]
        public string JobTitle { get; set; }

        [StringLength(ContactConsts.MaxCompanyNameLength, MinimumLength = ContactConsts.MinCompanyNameLength)]
        public string CompanyName { get; set; }

        public string Profile { get; set; }

        public string AiDataTag { get; set; }

        [StringLength(ContactConsts.MaxFacebookLength, MinimumLength = ContactConsts.MinFacebookLength)]
        public string Facebook { get; set; }

        [StringLength(ContactConsts.MaxLinkedInLength, MinimumLength = ContactConsts.MinLinkedInLength)]
        public string LinkedIn { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public bool Referred { get; set; }

        [StringLength(ContactConsts.MaxFaxLength, MinimumLength = ContactConsts.MinFaxLength)]
        public string Fax { get; set; }

        public bool Verified { get; set; }

        public int? Score { get; set; }

        public long? ReferredByUserId { get; set; }

        public long? CountryId { get; set; }

        public long? StateId { get; set; }

        public long? MembershipTypeId { get; set; }

    }
}