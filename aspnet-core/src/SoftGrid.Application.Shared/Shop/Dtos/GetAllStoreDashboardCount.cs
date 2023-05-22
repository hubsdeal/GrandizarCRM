namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreDashboardCount
    {
        public int TodaysOrders { get; set; } = 0;
        public decimal? TodaysOrdersTotalAmount { get; set; } = 0;
        public int LifetimeOrders { get; set; } = 0;
        public decimal? LifeTimeSales { get; set; } = 0;
        public int Customers { get; set; } = 0;
        public int Products { get; set; } = 0;
        public int Reviews { get; set; } = 0;
        public int StoreViewers { get; set; } = 0;
    }
}