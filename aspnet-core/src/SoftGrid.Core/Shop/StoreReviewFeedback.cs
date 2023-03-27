using SoftGrid.Shop;
using SoftGrid.CRM;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("StoreReviewFeedbacks")]
    public class StoreReviewFeedback : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual string ReplyText { get; set; }

        public virtual bool IsPublished { get; set; }

        public virtual long StoreReviewId { get; set; }

        [ForeignKey("StoreReviewId")]
        public StoreReview StoreReviewFk { get; set; }

        public virtual long? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

        public virtual long? RatingLikeId { get; set; }

        [ForeignKey("RatingLikeId")]
        public RatingLike RatingLikeFk { get; set; }

    }
}