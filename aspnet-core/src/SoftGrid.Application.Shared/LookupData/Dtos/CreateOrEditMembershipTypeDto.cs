using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditMembershipTypeDto : EntityDto<long?>
    {

        [Required]
        [StringLength(MembershipTypeConsts.MaxNameLength, MinimumLength = MembershipTypeConsts.MinNameLength)]
        public string Name { get; set; }

    }
}