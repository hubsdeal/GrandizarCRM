using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductCategoryDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool HasParentCategory { get; set; }

        public long? ParentCategoryId { get; set; }

        public string Url { get; set; }

        public string MetaTitle { get; set; }

        public string MetaKeywords { get; set; }

        public int? DisplaySequence { get; set; }

        public bool ProductOrService { get; set; }

        public long? MediaLibraryId { get; set; }

    }
}