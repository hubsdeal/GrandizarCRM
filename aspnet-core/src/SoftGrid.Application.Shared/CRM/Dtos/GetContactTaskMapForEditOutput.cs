using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetContactTaskMapForEditOutput
    {
        public CreateOrEditContactTaskMapDto ContactTaskMap { get; set; }

        public string ContactFullName { get; set; }

        public string TaskEventName { get; set; }

    }
}