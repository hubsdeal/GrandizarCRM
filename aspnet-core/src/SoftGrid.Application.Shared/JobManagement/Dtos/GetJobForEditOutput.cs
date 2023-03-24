using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.JobManagement.Dtos
{
    public class GetJobForEditOutput
    {
        public CreateOrEditJobDto Job { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

        public string ProductCategoryName { get; set; }

        public string CurrencyName { get; set; }

        public string BusinessName { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }

        public string CityName { get; set; }

        public string JobStatusTypeName { get; set; }

        public string StoreName { get; set; }

    }
}