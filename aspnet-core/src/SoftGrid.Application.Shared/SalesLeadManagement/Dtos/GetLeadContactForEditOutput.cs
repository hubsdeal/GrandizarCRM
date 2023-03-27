using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetLeadContactForEditOutput
    {
        public CreateOrEditLeadContactDto LeadContact { get; set; }

        public string LeadTitle { get; set; }

        public string ContactFullName { get; set; }

    }
}