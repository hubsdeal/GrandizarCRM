using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetContactDocumentForEditOutput
    {
        public CreateOrEditContactDocumentDto ContactDocument { get; set; }

        public string ContactFullName { get; set; }

        public string DocumentTypeName { get; set; }

    }
}