using SoftGrid.CMS.Enums;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.CMS
{
    [Table("Contents")]
    public class Content : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ContentConsts.MaxTitleLength, MinimumLength = ContentConsts.MinTitleLength)]
        public virtual string Title { get; set; }

        public virtual string Body { get; set; }

        public virtual bool Published { get; set; }

        public virtual DateTime? PublishedDate { get; set; }

        [StringLength(ContentConsts.MaxPublishTimeLength, MinimumLength = ContentConsts.MinPublishTimeLength)]
        public virtual string PublishTime { get; set; }

        [StringLength(ContentConsts.MaxSeoUrlLength, MinimumLength = ContentConsts.MinSeoUrlLength)]
        public virtual string SeoUrl { get; set; }

        [StringLength(ContentConsts.MaxSeoKeywordsLength, MinimumLength = ContentConsts.MinSeoKeywordsLength)]
        public virtual string SeoKeywords { get; set; }

        public virtual string SeoDescription { get; set; }

        public virtual ContentType ContentTypeId { get; set; }

        public virtual long? BannerMediaLibraryId { get; set; }

        [ForeignKey("BannerMediaLibraryId")]
        public MediaLibrary BannerMediaLibraryFk { get; set; }

    }
}