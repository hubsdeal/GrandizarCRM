using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditEmployeeDto : EntityDto<long?>
    {

        [Required]
        [StringLength(EmployeeConsts.MaxNameLength, MinimumLength = EmployeeConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(EmployeeConsts.MaxFirstNameLength, MinimumLength = EmployeeConsts.MinFirstNameLength)]
        public string FirstName { get; set; }

        [StringLength(EmployeeConsts.MaxLastNameLength, MinimumLength = EmployeeConsts.MinLastNameLength)]
        public string LastName { get; set; }

        [StringLength(EmployeeConsts.MaxFullAddressLength, MinimumLength = EmployeeConsts.MinFullAddressLength)]
        public string FullAddress { get; set; }

        [StringLength(EmployeeConsts.MaxAddressLength, MinimumLength = EmployeeConsts.MinAddressLength)]
        public string Address { get; set; }

        [StringLength(EmployeeConsts.MaxZipCodeLength, MinimumLength = EmployeeConsts.MinZipCodeLength)]
        public string ZipCode { get; set; }

        [StringLength(EmployeeConsts.MaxCityLength, MinimumLength = EmployeeConsts.MinCityLength)]
        public string City { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(EmployeeConsts.MaxMobileLength, MinimumLength = EmployeeConsts.MinMobileLength)]
        public string Mobile { get; set; }

        [StringLength(EmployeeConsts.MaxOfficePhoneLength, MinimumLength = EmployeeConsts.MinOfficePhoneLength)]
        public string OfficePhone { get; set; }

        [StringLength(EmployeeConsts.MaxPersonalEmailLength, MinimumLength = EmployeeConsts.MinPersonalEmailLength)]
        public string PersonalEmail { get; set; }

        [StringLength(EmployeeConsts.MaxBusinessEmailLength, MinimumLength = EmployeeConsts.MinBusinessEmailLength)]
        public string BusinessEmail { get; set; }

        [StringLength(EmployeeConsts.MaxJobTitleLength, MinimumLength = EmployeeConsts.MinJobTitleLength)]
        public string JobTitle { get; set; }

        [StringLength(EmployeeConsts.MaxCompanyNameLength, MinimumLength = EmployeeConsts.MinCompanyNameLength)]
        public string CompanyName { get; set; }

        public string Profile { get; set; }

        public DateTime? HireDate { get; set; }

        [StringLength(EmployeeConsts.MaxFacebookLength, MinimumLength = EmployeeConsts.MinFacebookLength)]
        public string Facebook { get; set; }

        [StringLength(EmployeeConsts.MaxLinkedInLength, MinimumLength = EmployeeConsts.MinLinkedInLength)]
        public string LinkedIn { get; set; }

        [StringLength(EmployeeConsts.MaxFaxLength, MinimumLength = EmployeeConsts.MinFaxLength)]
        public string Fax { get; set; }

        public Guid ProfilePictureId { get; set; }

        public bool CurrentEmployee { get; set; }

        public long? StateId { get; set; }

        public long? CountryId { get; set; }

        public long? ContactId { get; set; }

    }
}