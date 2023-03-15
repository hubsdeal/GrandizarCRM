using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditDocumentTypeDto : EntityDto<long?>
    {

        [Required]
        [StringLength(DocumentTypeConsts.MaxNameLength, MinimumLength = DocumentTypeConsts.MinNameLength)]
        public string Name { get; set; }

    }
}