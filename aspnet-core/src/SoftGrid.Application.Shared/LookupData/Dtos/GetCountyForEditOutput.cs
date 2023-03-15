using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class GetCountyForEditOutput
    {
        public CreateOrEditCountyDto County { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }

    }
}