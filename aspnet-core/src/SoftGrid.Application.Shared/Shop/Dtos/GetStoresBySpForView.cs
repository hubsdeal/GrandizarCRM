using System;
using System.Collections.Generic;
using System.Text;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoresBySpForView
    {
        public int TotalCount { get; set; }
        public int Published { get; set; }
        public int UnPublished { get; set; }
        public int Verified { get; set; }
        public int Favorite { get; set; }
        public int IsFavoriteOnly { get; set; } = 0;
        public List<StoreFromSpDto> Stores { get; set; }

        public GetStoresBySpForView()
        {
            Stores = new List<StoreFromSpDto>();
        }

    }

    public class StoreFromSpDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Guid StoreLogoLink { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public bool IsPublished { get; set; }

        public bool IsLocalOrOnlineStore { get; set; }

        public bool IsVerified { get; set; }

        public string ZipCode { get; set; }

        public string StateName { get; set; }

        public string CountryName { get; set; }

        public string Picture { get; set; }

        public int NumberOfProducts { get; set; }

        public string PrimaryCategoryName { get; set; }

        public long WishListId { get; set; }
    }
}
