using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetBusinessTagForEditOutput
    {
        public CreateOrEditBusinessTagDto BusinessTag { get; set; }

        public string BusinessName { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}