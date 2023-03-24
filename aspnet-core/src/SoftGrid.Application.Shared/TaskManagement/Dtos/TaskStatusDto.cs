using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.TaskManagement.Dtos
{
    public class TaskStatusDto : EntityDto<long>
    {
        public string Name { get; set; }

    }
}