using SoftGrid.CMS.Enums;

using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.CMS.Dtos
{
    public class ContentDto : EntityDto<long>
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public bool Published { get; set; }

        public DateTime? PublishedDate { get; set; }

        public string PublishTime { get; set; }

        public string SeoUrl { get; set; }

        public string SeoKeywords { get; set; }

        public string SeoDescription { get; set; }

        public ContentType ContentTypeId { get; set; }

        public long? BannerMediaLibraryId { get; set; }

    }
}