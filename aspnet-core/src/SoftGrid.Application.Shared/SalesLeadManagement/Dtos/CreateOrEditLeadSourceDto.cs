using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class CreateOrEditLeadSourceDto : EntityDto<long?>
    {

        [Required]
        [StringLength(LeadSourceConsts.MaxNameLength, MinimumLength = LeadSourceConsts.MinNameLength)]
        public string Name { get; set; }

    }
}