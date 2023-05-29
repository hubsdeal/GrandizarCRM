using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.TaskManagement.Dtos
{
    public class GetTaskWorkItemForEditOutput
    {
        public CreateOrEditTaskWorkItemDto TaskWorkItem { get; set; }

        public string TaskEventName { get; set; }

        public string EmployeeName { get; set; }

    }
}