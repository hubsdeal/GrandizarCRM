using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.JobManagement.Dtos
{
    public class GetJobTagForEditOutput
    {
        public CreateOrEditJobTagDto JobTag { get; set; }

        public string JobTitle { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}