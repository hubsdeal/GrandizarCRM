using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditReturnStatusDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ReturnStatusConsts.MaxNameLength, MinimumLength = ReturnStatusConsts.MinNameLength)]
        public string Name { get; set; }

    }
}