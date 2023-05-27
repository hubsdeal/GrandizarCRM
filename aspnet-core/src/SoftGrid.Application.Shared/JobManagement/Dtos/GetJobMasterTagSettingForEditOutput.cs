using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.JobManagement.Dtos
{
    public class GetJobMasterTagSettingForEditOutput
    {
        public CreateOrEditJobMasterTagSettingDto JobMasterTagSetting { get; set; }

        public string MasterTagName { get; set; }

        public string MasterTagCategoryName { get; set; }

    }
}