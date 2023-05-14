using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.TaskManagement.Dtos
{
    public class GetStoreTaskMapForEditOutput
    {
        public CreateOrEditStoreTaskMapDto StoreTaskMap { get; set; }

        public string StoreName { get; set; }

        public string TaskEventName { get; set; }

    }
}