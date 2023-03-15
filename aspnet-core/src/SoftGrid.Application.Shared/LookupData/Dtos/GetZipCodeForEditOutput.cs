using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class GetZipCodeForEditOutput
    {
        public CreateOrEditZipCodeDto ZipCode { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }

        public string CityName { get; set; }

        public string CountyName { get; set; }

    }
}