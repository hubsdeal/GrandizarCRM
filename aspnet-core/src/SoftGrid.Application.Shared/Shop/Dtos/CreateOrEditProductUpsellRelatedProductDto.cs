using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductUpsellRelatedProductDto : EntityDto<long?>
    {

        public long RelatedProductId { get; set; }

        public int? DisplaySequence { get; set; }

        public long PrimaryProductId { get; set; }

    }
}