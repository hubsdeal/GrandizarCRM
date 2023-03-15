using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductCategoriesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? HasParentCategoryFilter { get; set; }

        public long? MaxParentCategoryIdFilter { get; set; }
        public long? MinParentCategoryIdFilter { get; set; }

        public string UrlFilter { get; set; }

        public string MetaTitleFilter { get; set; }

        public string MetaKeywordsFilter { get; set; }

        public int? MaxDisplaySequenceFilter { get; set; }
        public int? MinDisplaySequenceFilter { get; set; }

        public int? ProductOrServiceFilter { get; set; }

        public string MediaLibraryNameFilter { get; set; }

    }
}