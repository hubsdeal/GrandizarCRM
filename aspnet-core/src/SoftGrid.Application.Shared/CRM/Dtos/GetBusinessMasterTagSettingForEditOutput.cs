using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetBusinessMasterTagSettingForEditOutput
    {
        public CreateOrEditBusinessMasterTagSettingDto BusinessMasterTagSetting { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}