using SoftGrid.CMS.Enums;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CMS.Dtos
{
    public class CreateOrEditContentDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ContentConsts.MaxTitleLength, MinimumLength = ContentConsts.MinTitleLength)]
        public string Title { get; set; }

        public string Body { get; set; }

        public bool Published { get; set; }

        public DateTime? PublishedDate { get; set; }

        [StringLength(ContentConsts.MaxPublishTimeLength, MinimumLength = ContentConsts.MinPublishTimeLength)]
        public string PublishTime { get; set; }

        [StringLength(ContentConsts.MaxSeoUrlLength, MinimumLength = ContentConsts.MinSeoUrlLength)]
        public string SeoUrl { get; set; }

        [StringLength(ContentConsts.MaxSeoKeywordsLength, MinimumLength = ContentConsts.MinSeoKeywordsLength)]
        public string SeoKeywords { get; set; }

        public string SeoDescription { get; set; }

        public ContentType ContentTypeId { get; set; }

        public long? BannerMediaLibraryId { get; set; }

    }
}