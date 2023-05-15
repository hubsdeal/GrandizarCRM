using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreRatingLikeLookupTableDto
    {
        public long Id { get; set; }

        public string DisplayName { get; set; }
    }

    public class ProductHubsLookupTableDto
    {
        public long? Id { get; set; }

        public string DisplayName { get; set; }

        public bool Selected { get; set; }
    }

    public class StoreHubsLookupTableDto
    {
        public long Id { get; set; }

        public string DisplayName { get; set; }

        public bool Selected { get; set; }
    }

    public class StorePrimaryCategoryLookupTableDto
    {
        public long Id { get; set; }

        public string DisplayName { get; set; }
    }

}