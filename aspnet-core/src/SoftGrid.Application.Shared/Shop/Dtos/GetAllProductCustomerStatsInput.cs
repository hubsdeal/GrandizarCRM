using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductCustomerStatsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? ClickedFilter { get; set; }

        public int? WishedOrFavoriteFilter { get; set; }

        public int? PurchasedFilter { get; set; }

        public int? MaxPurchasedQuantityFilter { get; set; }
        public int? MinPurchasedQuantityFilter { get; set; }

        public int? SharedFilter { get; set; }

        public string PageLinkFilter { get; set; }

        public int? AppOrWebFilter { get; set; }

        public string QuitFromLinkFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string HubNameFilter { get; set; }

        public string SocialMediaNameFilter { get; set; }

    }
}