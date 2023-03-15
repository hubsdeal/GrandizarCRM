using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetBusinessForEditOutput
    {
        public CreateOrEditBusinessDto Business { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }

        public string CityName { get; set; }

        public string MediaLibraryName { get; set; }

    }
}