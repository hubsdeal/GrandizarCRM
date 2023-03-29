namespace SoftGrid.Shop.Dtos
{
    public class GetProductCustomerQueryForViewDto
    {
        public ProductCustomerQueryDto ProductCustomerQuery { get; set; }

        public string ProductName { get; set; }

        public string ContactFullName { get; set; }

        public string EmployeeName { get; set; }

    }
}