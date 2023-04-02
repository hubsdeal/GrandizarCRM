namespace SoftGrid.DiscountManagement.Dtos
{
    public class GetCustomerWalletForViewDto
    {
        public CustomerWalletDto CustomerWallet { get; set; }

        public string ContactFullName { get; set; }

        public string UserName { get; set; }

        public string CurrencyName { get; set; }

    }
}