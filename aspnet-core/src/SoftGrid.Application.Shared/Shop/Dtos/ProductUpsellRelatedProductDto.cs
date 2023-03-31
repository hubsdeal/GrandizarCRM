using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductUpsellRelatedProductDto : EntityDto<long>
    {
        public long RelatedProductId { get; set; }

        public int? DisplaySequence { get; set; }

        public long PrimaryProductId { get; set; }

    }
}