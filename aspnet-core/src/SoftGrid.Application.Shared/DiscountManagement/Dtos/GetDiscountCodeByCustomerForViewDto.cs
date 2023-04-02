namespace SoftGrid.DiscountManagement.Dtos
{
    public class GetDiscountCodeByCustomerForViewDto
    {
        public DiscountCodeByCustomerDto DiscountCodeByCustomer { get; set; }

        public string DiscountCodeGeneratorName { get; set; }

        public string ContactFullName { get; set; }

    }
}