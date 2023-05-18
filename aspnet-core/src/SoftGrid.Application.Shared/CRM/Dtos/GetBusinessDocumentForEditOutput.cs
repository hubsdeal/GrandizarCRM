using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetBusinessDocumentForEditOutput
    {
        public CreateOrEditBusinessDocumentDto BusinessDocument { get; set; }

        public string BusinessName { get; set; }

        public string DocumentTypeName { get; set; }

    }
}