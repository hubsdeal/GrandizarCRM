using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class CreateOrEditLeadNoteDto : EntityDto<long?>
    {

        [Required]
        public string Notes { get; set; }

        public long? LeadId { get; set; }

    }
}