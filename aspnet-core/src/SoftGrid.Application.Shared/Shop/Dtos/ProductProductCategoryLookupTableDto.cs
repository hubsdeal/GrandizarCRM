using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductProductCategoryLookupTableDto
    {
        public long Id { get; set; }

        public string DisplayName { get; set; }

        public long? ParentcategoryId { get; set; }
    }
}