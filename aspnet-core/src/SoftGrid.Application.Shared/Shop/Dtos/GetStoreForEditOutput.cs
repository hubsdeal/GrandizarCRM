using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreForEditOutput
    {
        public CreateOrEditStoreDto Store { get; set; }

        public string MediaLibraryName { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }

        public string RatingLikeName { get; set; }

        public string MasterTagName { get; set; }

    }
}