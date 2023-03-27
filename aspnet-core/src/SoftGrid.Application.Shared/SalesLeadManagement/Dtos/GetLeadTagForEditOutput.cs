using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetLeadTagForEditOutput
    {
        public CreateOrEditLeadTagDto LeadTag { get; set; }

        public string LeadTitle { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}