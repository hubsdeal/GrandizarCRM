using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace SoftGrid.Shop.Dtos
{
    public class ProductProductCategoryLookupTableDto
    {
		public long Id { get; set; }

		public string DisplayName { get; set; }

        public long? ParentcategoryId { get; set; }
    }



    public class ProductProductCategoryLookupTableForPublicDto: ProductProductCategoryLookupTableDto
    {

        public string Url { get; set; }
        public Guid? PictureId { get; set; }
        public string Picture { get; set; }
        public int? DisplaySequence { get; set; }
        public int NumberOfProducts { get; set; }
    }

    public class ProductCategoryDirectoryPublicViewBySp
    {
        public List<ProductProductCategoryLookupTableForPublicDto> Categories { get; set; }
    }

    public class ProductTemplateForDropdownViewDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class GetAllTemplatesBySp
    {
        public List<ProductTemplateForDropdownViewDto> Templates { get; set; }

        public GetAllTemplatesBySp()
        {
            Templates = new List<ProductTemplateForDropdownViewDto>();
        }
    }
}