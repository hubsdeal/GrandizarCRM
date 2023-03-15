using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditContractTypeDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ContractTypeConsts.MaxNameLength, MinimumLength = ContractTypeConsts.MinNameLength)]
        public string Name { get; set; }

    }
}