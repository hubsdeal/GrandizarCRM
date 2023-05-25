using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductTaskMapForEditOutput
    {
        public CreateOrEditProductTaskMapDto ProductTaskMap { get; set; }

        public string ProductName { get; set; }

        public string TaskEventName { get; set; }

        public string ProductCategoryName { get; set; }

    }
}