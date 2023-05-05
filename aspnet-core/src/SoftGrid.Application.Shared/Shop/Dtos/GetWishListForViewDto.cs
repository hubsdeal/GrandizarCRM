namespace SoftGrid.Shop.Dtos
{
    public class GetWishListForViewDto
    {
        public WishListDto WishList { get; set; }

        public string ContactFullName { get; set; }

        public string ProductName { get; set; }

        public string StoreName { get; set; }

    }
}