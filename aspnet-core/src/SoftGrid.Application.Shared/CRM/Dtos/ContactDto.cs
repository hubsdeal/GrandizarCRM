using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.CRM.Dtos
{
    public class ContactDto : EntityDto<long>
    {
        public string FullName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullAddress { get; set; }

        public string Address { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Mobile { get; set; }

        public string OfficePhone { get; set; }

        public string CountryCode { get; set; }

        public string PersonalEmail { get; set; }

        public string BusinessEmail { get; set; }

        public string JobTitle { get; set; }

        public string CompanyName { get; set; }

        public string AiDataTag { get; set; }

        public string Facebook { get; set; }

        public string LinkedIn { get; set; }

        public bool Referred { get; set; }

        public bool Verified { get; set; }

        public int? Score { get; set; }

        public long? ReferredByUserId { get; set; }

        public long? CountryId { get; set; }

        public long? StateId { get; set; }

        public long? MembershipTypeId { get; set; }

        public bool? IsApplicant { get; set; }
        public bool? Active { get; set; }
        public Guid? PictureId { get; set; }

    }
}