using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CMS.Dtos
{
    public class GetAllContentsForExcelInput
    {
        public string Filter { get; set; }

        public string TitleFilter { get; set; }

        public string BodyFilter { get; set; }

        public int? PublishedFilter { get; set; }

        public DateTime? MaxPublishedDateFilter { get; set; }
        public DateTime? MinPublishedDateFilter { get; set; }

        public string PublishTimeFilter { get; set; }

        public string SeoUrlFilter { get; set; }

        public string SeoKeywordsFilter { get; set; }

        public string SeoDescriptionFilter { get; set; }

        public int? ContentTypeIdFilter { get; set; }

        public string MediaLibraryNameFilter { get; set; }

    }
}