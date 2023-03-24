using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreSalesAlertDto : EntityDto<long>
    {
        public string Message { get; set; }

        public bool Current { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public long StoreId { get; set; }

    }
}