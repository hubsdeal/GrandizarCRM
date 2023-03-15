using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class GetCityForEditOutput
    {
        public CreateOrEditCityDto City { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }

        public string CountyName { get; set; }

    }
}