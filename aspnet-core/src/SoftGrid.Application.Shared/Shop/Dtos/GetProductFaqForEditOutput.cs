using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductFaqForEditOutput
    {
        public CreateOrEditProductFaqDto ProductFaq { get; set; }

        public string ProductName { get; set; }

    }
}