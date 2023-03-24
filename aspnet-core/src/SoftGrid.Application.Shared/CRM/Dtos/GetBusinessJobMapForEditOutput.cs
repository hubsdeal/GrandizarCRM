using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetBusinessJobMapForEditOutput
    {
        public CreateOrEditBusinessJobMapDto BusinessJobMap { get; set; }

        public string BusinessName { get; set; }

        public string JobTitle { get; set; }

    }
}