using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductCategoryDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ProductCategoryConsts.MaxNameLength, MinimumLength = ProductCategoryConsts.MinNameLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool HasParentCategory { get; set; }

        public long? ParentCategoryId { get; set; }

        [StringLength(ProductCategoryConsts.MaxUrlLength, MinimumLength = ProductCategoryConsts.MinUrlLength)]
        public string Url { get; set; }

        [StringLength(ProductCategoryConsts.MaxMetaTitleLength, MinimumLength = ProductCategoryConsts.MinMetaTitleLength)]
        public string MetaTitle { get; set; }

        [StringLength(ProductCategoryConsts.MaxMetaKeywordsLength, MinimumLength = ProductCategoryConsts.MinMetaKeywordsLength)]
        public string MetaKeywords { get; set; }

        public int? DisplaySequence { get; set; }

        public bool ProductOrService { get; set; }

        public long? MediaLibraryId { get; set; }

    }
}