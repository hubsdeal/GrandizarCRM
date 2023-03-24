using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.CRM;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.CRM
{
    [Table("Employees")]
    public class Employee : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(EmployeeConsts.MaxNameLength, MinimumLength = EmployeeConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(EmployeeConsts.MaxFirstNameLength, MinimumLength = EmployeeConsts.MinFirstNameLength)]
        public virtual string FirstName { get; set; }

        [StringLength(EmployeeConsts.MaxLastNameLength, MinimumLength = EmployeeConsts.MinLastNameLength)]
        public virtual string LastName { get; set; }

        [StringLength(EmployeeConsts.MaxFullAddressLength, MinimumLength = EmployeeConsts.MinFullAddressLength)]
        public virtual string FullAddress { get; set; }

        [StringLength(EmployeeConsts.MaxAddressLength, MinimumLength = EmployeeConsts.MinAddressLength)]
        public virtual string Address { get; set; }

        [StringLength(EmployeeConsts.MaxZipCodeLength, MinimumLength = EmployeeConsts.MinZipCodeLength)]
        public virtual string ZipCode { get; set; }

        [StringLength(EmployeeConsts.MaxCityLength, MinimumLength = EmployeeConsts.MinCityLength)]
        public virtual string City { get; set; }

        public virtual DateTime? DateOfBirth { get; set; }

        [StringLength(EmployeeConsts.MaxMobileLength, MinimumLength = EmployeeConsts.MinMobileLength)]
        public virtual string Mobile { get; set; }

        [StringLength(EmployeeConsts.MaxOfficePhoneLength, MinimumLength = EmployeeConsts.MinOfficePhoneLength)]
        public virtual string OfficePhone { get; set; }

        [StringLength(EmployeeConsts.MaxPersonalEmailLength, MinimumLength = EmployeeConsts.MinPersonalEmailLength)]
        public virtual string PersonalEmail { get; set; }

        [StringLength(EmployeeConsts.MaxBusinessEmailLength, MinimumLength = EmployeeConsts.MinBusinessEmailLength)]
        public virtual string BusinessEmail { get; set; }

        [StringLength(EmployeeConsts.MaxJobTitleLength, MinimumLength = EmployeeConsts.MinJobTitleLength)]
        public virtual string JobTitle { get; set; }

        [StringLength(EmployeeConsts.MaxCompanyNameLength, MinimumLength = EmployeeConsts.MinCompanyNameLength)]
        public virtual string CompanyName { get; set; }

        public virtual string Profile { get; set; }

        public virtual DateTime? HireDate { get; set; }

        [StringLength(EmployeeConsts.MaxFacebookLength, MinimumLength = EmployeeConsts.MinFacebookLength)]
        public virtual string Facebook { get; set; }

        [StringLength(EmployeeConsts.MaxLinkedInLength, MinimumLength = EmployeeConsts.MinLinkedInLength)]
        public virtual string LinkedIn { get; set; }

        [StringLength(EmployeeConsts.MaxFaxLength, MinimumLength = EmployeeConsts.MinFaxLength)]
        public virtual string Fax { get; set; }

        public virtual Guid ProfilePictureId { get; set; }

        public virtual bool CurrentEmployee { get; set; }

        public virtual long? StateId { get; set; }

        [ForeignKey("StateId")]
        public State StateFk { get; set; }

        public virtual long? CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

        public virtual long? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

    }
}