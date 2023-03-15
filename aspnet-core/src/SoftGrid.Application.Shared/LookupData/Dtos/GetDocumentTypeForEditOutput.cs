using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class GetDocumentTypeForEditOutput
    {
        public CreateOrEditDocumentTypeDto DocumentType { get; set; }

    }
}