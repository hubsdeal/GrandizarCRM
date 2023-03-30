using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class SocialMediaDto : EntityDto<long>
    {
        public string Name { get; set; }

    }
}