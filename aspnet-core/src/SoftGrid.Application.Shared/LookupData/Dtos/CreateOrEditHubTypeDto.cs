using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditHubTypeDto : EntityDto<long?>
    {

        [Required]
        [StringLength(HubTypeConsts.MaxNameLength, MinimumLength = HubTypeConsts.MinNameLength)]
        public string Name { get; set; }

    }
}