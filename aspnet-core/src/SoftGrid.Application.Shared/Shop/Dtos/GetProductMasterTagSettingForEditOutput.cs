using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductMasterTagSettingForEditOutput
    {
        public CreateOrEditProductMasterTagSettingDto ProductMasterTagSetting { get; set; }

        public string ProductCategoryName { get; set; }

        public string MasterTagCategoryName { get; set; }

    }
}