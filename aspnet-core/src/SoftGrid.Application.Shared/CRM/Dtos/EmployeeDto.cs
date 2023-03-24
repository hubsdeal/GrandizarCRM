using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.CRM.Dtos
{
    public class EmployeeDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullAddress { get; set; }

        public string Address { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Mobile { get; set; }

        public string OfficePhone { get; set; }

        public string PersonalEmail { get; set; }

        public string BusinessEmail { get; set; }

        public string JobTitle { get; set; }

        public string CompanyName { get; set; }

        public string Profile { get; set; }

        public DateTime? HireDate { get; set; }

        public string Facebook { get; set; }

        public string LinkedIn { get; set; }

        public string Fax { get; set; }

        public Guid ProfilePictureId { get; set; }

        public bool CurrentEmployee { get; set; }

        public long? StateId { get; set; }

        public long? CountryId { get; set; }

        public long? ContactId { get; set; }

    }
}