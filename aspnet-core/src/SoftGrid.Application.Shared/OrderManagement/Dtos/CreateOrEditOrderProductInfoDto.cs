using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class CreateOrEditOrderProductInfoDto : EntityDto<long?>
    {

        public int? Quantity { get; set; }

        public double? UnitPrice { get; set; }

        public double? ByProductDiscountAmount { get; set; }

        public double? ByProductDiscountPercentage { get; set; }

        public double? ByProductTaxAmount { get; set; }

        public double? ByProductTotalAmount { get; set; }

        public long? OrderId { get; set; }

        public long? StoreId { get; set; }

        public long? ProductId { get; set; }

        public long? MeasurementUnitId { get; set; }

    }
}