using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderProductVariantForEditOutput
    {
        public CreateOrEditOrderProductVariantDto OrderProductVariant { get; set; }

        public string ProductVariantCategoryName { get; set; }

        public string ProductVariantName { get; set; }

    }
}