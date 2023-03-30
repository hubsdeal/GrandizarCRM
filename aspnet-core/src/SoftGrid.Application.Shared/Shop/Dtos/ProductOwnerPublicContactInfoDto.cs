using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductOwnerPublicContactInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string ShortBio { get; set; }

        public bool Publish { get; set; }

        public Guid PhotoId { get; set; }

        public long? ContactId { get; set; }

        public long? ProductId { get; set; }

        public long? StoreId { get; set; }

        public long? UserId { get; set; }

    }
}