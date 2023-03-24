using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetBusinessContactMapForEditOutput
    {
        public CreateOrEditBusinessContactMapDto BusinessContactMap { get; set; }

        public string BusinessName { get; set; }

        public string ContactFullName { get; set; }

    }
}