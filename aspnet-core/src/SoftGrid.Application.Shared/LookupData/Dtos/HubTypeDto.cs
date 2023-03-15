using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class HubTypeDto : EntityDto<long>
    {
        public string Name { get; set; }

    }
}