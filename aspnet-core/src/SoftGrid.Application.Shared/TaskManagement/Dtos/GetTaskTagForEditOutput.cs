using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.TaskManagement.Dtos
{
    public class GetTaskTagForEditOutput
    {
        public CreateOrEditTaskTagDto TaskTag { get; set; }

        public string TaskEventName { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}