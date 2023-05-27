using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetContactMasterTagSettingForEditOutput
    {
        public CreateOrEditContactMasterTagSettingDto ContactMasterTagSetting { get; set; }

        public string MasterTagName { get; set; }

        public string MasterTagCategoryName { get; set; }

    }
}