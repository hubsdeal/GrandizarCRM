using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.JobManagement.Dtos
{
    public class GetJobDocumentForEditOutput
    {
        public CreateOrEditJobDocumentDto JobDocument { get; set; }

        public string JobTitle { get; set; }

        public string DocumentTypeName { get; set; }

    }
}