using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetContactTagForEditOutput
    {
        public CreateOrEditContactTagDto ContactTag { get; set; }

        public string ContactFullName { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}