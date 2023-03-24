using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreLocationForEditOutput
    {
        public CreateOrEditStoreLocationDto StoreLocation { get; set; }

        public string CityName { get; set; }

        public string StateName { get; set; }

        public string CountryName { get; set; }

        public string StoreName { get; set; }

    }
}