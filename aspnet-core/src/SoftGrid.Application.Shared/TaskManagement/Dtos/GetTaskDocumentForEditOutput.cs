using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.TaskManagement.Dtos
{
    public class GetTaskDocumentForEditOutput
    {
        public CreateOrEditTaskDocumentDto TaskDocument { get; set; }

        public string TaskEventName { get; set; }

        public string DocumentTypeName { get; set; }

    }
}