using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetLeadNoteForEditOutput
    {
        public CreateOrEditLeadNoteDto LeadNote { get; set; }

        public string LeadTitle { get; set; }

    }
}