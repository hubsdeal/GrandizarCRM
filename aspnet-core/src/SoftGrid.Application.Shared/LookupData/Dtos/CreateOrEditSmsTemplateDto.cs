using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditSmsTemplateDto : EntityDto<long?>
    {

        [Required]
        [StringLength(SmsTemplateConsts.MaxTitleLength, MinimumLength = SmsTemplateConsts.MinTitleLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public bool Published { get; set; }

    }
}