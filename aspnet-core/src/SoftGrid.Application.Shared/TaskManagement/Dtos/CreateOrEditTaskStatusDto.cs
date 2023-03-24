using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.TaskManagement.Dtos
{
    public class CreateOrEditTaskStatusDto : EntityDto<long?>
    {

        [Required]
        [StringLength(TaskStatusConsts.MaxNameLength, MinimumLength = TaskStatusConsts.MinNameLength)]
        public string Name { get; set; }

    }
}