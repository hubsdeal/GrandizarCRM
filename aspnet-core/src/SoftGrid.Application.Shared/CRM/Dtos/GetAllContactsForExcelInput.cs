using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllContactsForExcelInput
    {
        public string Filter { get; set; }

        public string FullNameFilter { get; set; }

        public string FirstNameFilter { get; set; }

        public string LastNameFilter { get; set; }

        public string FullAddressFilter { get; set; }

        public string AddressFilter { get; set; }

        public string ZipCodeFilter { get; set; }

        public string CityFilter { get; set; }

        public DateTime? MaxDateOfBirthFilter { get; set; }
        public DateTime? MinDateOfBirthFilter { get; set; }

        public string CountryCodeFilter { get; set; }

        public string PersonalEmailFilter { get; set; }

        public string BusinessEmailFilter { get; set; }

        public string JobTitleFilter { get; set; }

        public string CompanyNameFilter { get; set; }

        public string ProfileFilter { get; set; }

        public string AiDataTagFilter { get; set; }

        public string FacebookFilter { get; set; }

        public string LinkedInFilter { get; set; }

        public int? ReferredFilter { get; set; }

        public int? VerifiedFilter { get; set; }

        public int? MaxScoreFilter { get; set; }
        public int? MinScoreFilter { get; set; }

        public string UserNameFilter { get; set; }

        public string CountryNameFilter { get; set; }

        public string StateNameFilter { get; set; }

        public string MembershipTypeNameFilter { get; set; }

    }
}