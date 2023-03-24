using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.TaskManagement.Dtos
{
    public class GetTaskEventForEditOutput
    {
        public CreateOrEditTaskEventDto TaskEvent { get; set; }

        public string TaskStatusName { get; set; }

    }
}