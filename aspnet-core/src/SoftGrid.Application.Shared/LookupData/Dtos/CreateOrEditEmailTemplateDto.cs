using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditEmailTemplateDto : EntityDto<long?>
    {

        [Required]
        [StringLength(EmailTemplateConsts.MaxSubjectLength, MinimumLength = EmailTemplateConsts.MinSubjectLength)]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        public bool Published { get; set; }

    }
}