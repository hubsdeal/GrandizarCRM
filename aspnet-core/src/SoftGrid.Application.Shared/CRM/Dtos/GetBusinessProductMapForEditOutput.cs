using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetBusinessProductMapForEditOutput
    {
        public CreateOrEditBusinessProductMapDto BusinessProductMap { get; set; }

        public string BusinessName { get; set; }

        public string ProductName { get; set; }

    }
}