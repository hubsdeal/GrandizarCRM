using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductForEditOutput
    {
        public CreateOrEditProductDto Product { get; set; }

        public string ProductCategoryName { get; set; }

        public string MediaLibraryName { get; set; }

        public string MeasurementUnitName { get; set; }

        public string CurrencyName { get; set; }

        public string RatingLikeName { get; set; }

    }
}