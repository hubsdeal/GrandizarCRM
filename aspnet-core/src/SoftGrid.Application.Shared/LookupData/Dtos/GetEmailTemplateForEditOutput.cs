using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class GetEmailTemplateForEditOutput
    {
        public CreateOrEditEmailTemplateDto EmailTemplate { get; set; }

    }
}