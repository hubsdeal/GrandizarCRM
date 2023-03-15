using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class GetMasterTagCategoryForEditOutput
    {
        public CreateOrEditMasterTagCategoryDto MasterTagCategory { get; set; }

    }
}