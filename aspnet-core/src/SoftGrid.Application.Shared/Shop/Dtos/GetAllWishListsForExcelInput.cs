using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllWishListsForExcelInput
    {
        public string Filter { get; set; }

        public DateTime? MaxDateFilter { get; set; }
        public DateTime? MinDateFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string StoreNameFilter { get; set; }

    }
}