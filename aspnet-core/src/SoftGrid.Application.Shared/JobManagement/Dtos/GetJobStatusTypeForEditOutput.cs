using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.JobManagement.Dtos
{
    public class GetJobStatusTypeForEditOutput
    {
        public CreateOrEditJobStatusTypeDto JobStatusType { get; set; }

    }
}