using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class GetAllDiscountCodeMapsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DiscountCodeGeneratorNameFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string MembershipTypeNameFilter { get; set; }

    }
}