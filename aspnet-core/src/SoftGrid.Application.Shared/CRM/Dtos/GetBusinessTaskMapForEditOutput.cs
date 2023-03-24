using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetBusinessTaskMapForEditOutput
    {
        public CreateOrEditBusinessTaskMapDto BusinessTaskMap { get; set; }

        public string BusinessName { get; set; }

        public string TaskEventName { get; set; }

    }
}