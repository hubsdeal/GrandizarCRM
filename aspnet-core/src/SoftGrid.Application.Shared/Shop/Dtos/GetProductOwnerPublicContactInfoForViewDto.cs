namespace SoftGrid.Shop.Dtos
{
    public class GetProductOwnerPublicContactInfoForViewDto
    {
        public ProductOwnerPublicContactInfoDto ProductOwnerPublicContactInfo { get; set; }

        public string ContactFullName { get; set; }

        public string ProductName { get; set; }

        public string StoreName { get; set; }

        public string UserName { get; set; }

    }
}