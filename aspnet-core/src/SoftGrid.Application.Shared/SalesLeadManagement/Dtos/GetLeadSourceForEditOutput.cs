using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetLeadSourceForEditOutput
    {
        public CreateOrEditLeadSourceDto LeadSource { get; set; }

    }
}