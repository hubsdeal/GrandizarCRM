using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreSalesAlertDto : EntityDto<long?>
    {

        [StringLength(StoreSalesAlertConsts.MaxMessageLength, MinimumLength = StoreSalesAlertConsts.MinMessageLength)]
        public string Message { get; set; }

        public bool Current { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public long StoreId { get; set; }

    }
}