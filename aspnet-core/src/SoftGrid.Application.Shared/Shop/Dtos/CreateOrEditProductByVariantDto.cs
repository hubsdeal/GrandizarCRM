using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductByVariantDto : EntityDto<long?>
    {

        public double? Price { get; set; }

        public int? DisplaySequence { get; set; }

        public string Description { get; set; }

        public long? ProductId { get; set; }

        public long? ProductVariantId { get; set; }

        public long? ProductVariantCategoryId { get; set; }

        public long? MediaLibraryId { get; set; }

    }
}