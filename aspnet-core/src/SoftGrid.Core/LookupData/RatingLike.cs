using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("RatingLikes")]
    public class RatingLike : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(RatingLikeConsts.MaxNameLength, MinimumLength = RatingLikeConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual int? Score { get; set; }

        [StringLength(RatingLikeConsts.MaxIconLinkLength, MinimumLength = RatingLikeConsts.MinIconLinkLength)]
        public virtual string IconLink { get; set; }

    }
}