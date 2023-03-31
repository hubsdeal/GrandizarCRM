using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class CreateOrEditOrderProductVariantDto : EntityDto<long?>
    {

        public double? Price { get; set; }

        public long? OrderProductInfoId { get; set; }

        public long? ProductVariantCategoryId { get; set; }

        public long? ProductVariantId { get; set; }

    }
}