using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllEmployeesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string FirstNameFilter { get; set; }

        public string LastNameFilter { get; set; }

        public string FullAddressFilter { get; set; }

        public string AddressFilter { get; set; }

        public string ZipCodeFilter { get; set; }

        public string CityFilter { get; set; }

        public DateTime? MaxDateOfBirthFilter { get; set; }
        public DateTime? MinDateOfBirthFilter { get; set; }

        public string MobileFilter { get; set; }

        public string OfficePhoneFilter { get; set; }

        public string PersonalEmailFilter { get; set; }

        public string BusinessEmailFilter { get; set; }

        public string JobTitleFilter { get; set; }

        public string CompanyNameFilter { get; set; }

        public string ProfileFilter { get; set; }

        public DateTime? MaxHireDateFilter { get; set; }
        public DateTime? MinHireDateFilter { get; set; }

        public string FacebookFilter { get; set; }

        public string LinkedInFilter { get; set; }

        public string FaxFilter { get; set; }

        public Guid? ProfilePictureIdFilter { get; set; }

        public int? CurrentEmployeeFilter { get; set; }

        public string StateNameFilter { get; set; }

        public string CountryNameFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

    }
}