using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

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

        public string Picture { get; set; }

        public List<string> StoreTags { get; set; }

        public GetStoreForEditOutput()
        {
            StoreTags = new List<string>();
        }

        public int NumberOfTasks { get; set; }

        public int NumberOfNotes { get; set; }
        public int NumberOfRatings { get; set; }
        public double? RatingScore { get; set; }
        public string PrimaryCategoryName { get; set; }

        public string Productcategoryname { get; set; }

       

    }
}