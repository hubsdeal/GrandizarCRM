using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class GetAllDiscountCodeByCustomersInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DiscountCodeGeneratorNameFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

    }
}