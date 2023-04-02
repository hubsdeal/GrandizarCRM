using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class DiscountCodeUserHistoryDto : EntityDto<long>
    {
        public DateTime? Amount { get; set; }

        public DateTime? Date { get; set; }

        public long? DiscountCodeGeneratorId { get; set; }

        public long? OrderId { get; set; }

        public long? ContactId { get; set; }

    }
}