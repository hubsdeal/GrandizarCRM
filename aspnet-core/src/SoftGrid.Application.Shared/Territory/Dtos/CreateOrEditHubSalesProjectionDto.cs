using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class CreateOrEditHubSalesProjectionDto : EntityDto<long?>
    {

        public int? DurationTypeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public double? ExpectedSalesAmount { get; set; }

        public double? ActualSalesAmount { get; set; }

        public long HubId { get; set; }

        public long? ProductCategoryId { get; set; }

        public long? StoreId { get; set; }

        public long? CurrencyId { get; set; }

    }
}