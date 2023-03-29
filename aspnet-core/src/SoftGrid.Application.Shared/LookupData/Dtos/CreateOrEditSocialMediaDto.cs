using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditSocialMediaDto : EntityDto<long?>
    {

        [Required]
        [StringLength(SocialMediaConsts.MaxNameLength, MinimumLength = SocialMediaConsts.MinNameLength)]
        public string Name { get; set; }

    }
}