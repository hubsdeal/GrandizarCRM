using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductCategoryForEditOutput
    {
        public CreateOrEditProductCategoryDto ProductCategory { get; set; }

        public string MediaLibraryName { get; set; }

    }
}