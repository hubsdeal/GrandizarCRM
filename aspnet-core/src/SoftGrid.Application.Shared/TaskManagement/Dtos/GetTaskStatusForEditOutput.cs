using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.TaskManagement.Dtos
{
    public class GetTaskStatusForEditOutput
    {
        public CreateOrEditTaskStatusDto TaskStatus { get; set; }

    }
}