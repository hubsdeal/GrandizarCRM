using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreMasterTagSettingForEditOutput
    {
        public CreateOrEditStoreMasterTagSettingDto StoreMasterTagSetting { get; set; }

        public string StoreTagSettingCategoryName { get; set; }

        public string MasterTagCategoryName { get; set; }

    }
}