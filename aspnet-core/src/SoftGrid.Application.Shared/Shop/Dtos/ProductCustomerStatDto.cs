using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductCustomerStatDto : EntityDto<long>
    {
        public bool Clicked { get; set; }

        public bool WishedOrFavorite { get; set; }

        public bool Purchased { get; set; }

        public int? PurchasedQuantity { get; set; }

        public bool Shared { get; set; }

        public string PageLink { get; set; }

        public bool AppOrWeb { get; set; }

        public string QuitFromLink { get; set; }

        public long? ProductId { get; set; }

        public long? ContactId { get; set; }

        public long? StoreId { get; set; }

        public long? HubId { get; set; }

        public long? SocialMediaId { get; set; }

    }
}