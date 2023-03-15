using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class EmailTemplateDto : EntityDto<long>
    {
        public string Subject { get; set; }

        public string Content { get; set; }

        public bool Published { get; set; }

    }
}