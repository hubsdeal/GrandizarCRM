using System;
using Abp.Application.Services.Dto;
using JetBrains.Annotations;

namespace SoftGrid.LookupData.Dtos
{
    public class SmsTemplateDto : EntityDto<long>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public bool Published { get; set; }

    }

  
}