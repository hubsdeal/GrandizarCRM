using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class ContractTypeDto : EntityDto<long>
    {
        public string Name { get; set; }

    }
}