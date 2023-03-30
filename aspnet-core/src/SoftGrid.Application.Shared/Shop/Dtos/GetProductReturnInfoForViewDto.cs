namespace SoftGrid.Shop.Dtos
{
    public class GetProductReturnInfoForViewDto
    {
        public ProductReturnInfoDto ProductReturnInfo { get; set; }

        public string ProductName { get; set; }

        public string ReturnTypeName { get; set; }

        public string ReturnStatusName { get; set; }

    }
}