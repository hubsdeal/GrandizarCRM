namespace SoftGrid.DiscountManagement.Dtos
{
    public class GetDiscountCodeUserHistoryForViewDto
    {
        public DiscountCodeUserHistoryDto DiscountCodeUserHistory { get; set; }

        public string DiscountCodeGeneratorName { get; set; }

        public string OrderInvoiceNumber { get; set; }

        public string ContactFullName { get; set; }

    }
}