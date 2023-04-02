using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class GetDiscountCodeUserHistoryForEditOutput
    {
        public CreateOrEditDiscountCodeUserHistoryDto DiscountCodeUserHistory { get; set; }

        public string DiscountCodeGeneratorName { get; set; }

        public string OrderInvoiceNumber { get; set; }

        public string ContactFullName { get; set; }

    }
}