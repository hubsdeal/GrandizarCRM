using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductOwnerPublicContactInfoDto : EntityDto<long?>
    {

        [StringLength(ProductOwnerPublicContactInfoConsts.MaxNameLength, MinimumLength = ProductOwnerPublicContactInfoConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(ProductOwnerPublicContactInfoConsts.MaxMobileLength, MinimumLength = ProductOwnerPublicContactInfoConsts.MinMobileLength)]
        public string Mobile { get; set; }

        [StringLength(ProductOwnerPublicContactInfoConsts.MaxEmailLength, MinimumLength = ProductOwnerPublicContactInfoConsts.MinEmailLength)]
        public string Email { get; set; }

        [StringLength(ProductOwnerPublicContactInfoConsts.MaxShortBioLength, MinimumLength = ProductOwnerPublicContactInfoConsts.MinShortBioLength)]
        public string ShortBio { get; set; }

        public bool Publish { get; set; }

        public Guid PhotoId { get; set; }

        public long? ContactId { get; set; }

        public long? ProductId { get; set; }

        public long? StoreId { get; set; }

        public long? UserId { get; set; }

    }
}