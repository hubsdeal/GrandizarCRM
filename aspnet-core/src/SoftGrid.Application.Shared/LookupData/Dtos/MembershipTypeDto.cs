using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class MembershipTypeDto : EntityDto<long>
    {
        public string Name { get; set; }

    }
}