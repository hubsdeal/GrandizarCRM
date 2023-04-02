using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class GetAllDiscountCodeUserHistoriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public DateTime? MaxAmountFilter { get; set; }
        public DateTime? MinAmountFilter { get; set; }

        public DateTime? MaxDateFilter { get; set; }
        public DateTime? MinDateFilter { get; set; }

        public string DiscountCodeGeneratorNameFilter { get; set; }

        public string OrderInvoiceNumberFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

    }
}