using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.TaskManagement.Dtos
{
    public class GetAllStoreTaskMapsForExcelInput
    {
        public string Filter { get; set; }

        public string StoreNameFilter { get; set; }

        public string TaskEventNameFilter { get; set; }

    }
}