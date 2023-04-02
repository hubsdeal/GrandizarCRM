namespace SoftGrid.DiscountManagement.Dtos
{
    public class GetDiscountCodeMapForViewDto
    {
        public DiscountCodeMapDto DiscountCodeMap { get; set; }

        public string DiscountCodeGeneratorName { get; set; }

        public string StoreName { get; set; }

        public string ProductName { get; set; }

        public string MembershipTypeName { get; set; }

    }
}