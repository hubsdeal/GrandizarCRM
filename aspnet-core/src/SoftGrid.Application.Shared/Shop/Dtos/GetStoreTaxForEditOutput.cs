using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreTaxForEditOutput
    {
        public CreateOrEditStoreTaxDto StoreTax { get; set; }

        public string StoreName { get; set; }

    }
}