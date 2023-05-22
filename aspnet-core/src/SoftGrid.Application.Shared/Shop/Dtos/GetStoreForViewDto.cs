using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreForViewDto
    {
        public StoreDto Store { get; set; }

        public string MediaLibraryName { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }

        public string RatingLikeName { get; set; }

        public string MasterTagName { get; set; }

    }
}