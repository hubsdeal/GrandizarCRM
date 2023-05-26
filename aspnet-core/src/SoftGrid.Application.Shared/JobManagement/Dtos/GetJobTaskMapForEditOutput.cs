using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.JobManagement.Dtos
{
    public class GetJobTaskMapForEditOutput
    {
        public CreateOrEditJobTaskMapDto JobTaskMap { get; set; }

        public string JobTitle { get; set; }

        public string TaskEventName { get; set; }

    }
}