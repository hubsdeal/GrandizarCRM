using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditReturnTypeDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ReturnTypeConsts.MaxNameLength, MinimumLength = ReturnTypeConsts.MinNameLength)]
        public string Name { get; set; }

    }
}