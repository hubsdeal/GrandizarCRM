namespace SoftGrid.OrderManagement
{
    public class OrderPaymentInfoConsts
    {

        public const int MinBillingAddressLength = 1;
        public const int MaxBillingAddressLength = 1024;

        public const int MinBillingCityLength = 1;
        public const int MaxBillingCityLength = 128;

        public const int MinBillingStateLength = 1;
        public const int MaxBillingStateLength = 128;

        public const int MinBillingZipCodeLength = 1;
        public const int MaxBillingZipCodeLength = 50;

        public const int MinCardNameLength = 1;
        public const int MaxCardNameLength = 256;

        public const int MinCardNumberLength = 1;
        public const int MaxCardNumberLength = 128;

        public const int MinCardCvvLength = 1;
        public const int MaxCardCvvLength = 50;

        public const int MinCardExpirationMonthLength = 1;
        public const int MaxCardExpirationMonthLength = 128;

        public const int MinCardExpirationYearLength = 1;
        public const int MaxCardExpirationYearLength = 128;

        public const int MinPaidTimeLength = 1;
        public const int MaxPaidTimeLength = 50;

    }
}