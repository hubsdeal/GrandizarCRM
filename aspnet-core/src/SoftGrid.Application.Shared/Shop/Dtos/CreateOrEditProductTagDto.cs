using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductTagDto : EntityDto<long?>
    {

        [StringLength(ProductTagConsts.MaxCustomTagLength, MinimumLength = ProductTagConsts.MinCustomTagLength)]
        public string CustomTag { get; set; }

        [StringLength(ProductTagConsts.MaxTagValueLength, MinimumLength = ProductTagConsts.MinTagValueLength)]
        public string TagValue { get; set; }

        public bool Verified { get; set; }

        public int? Sequence { get; set; }

        public long ProductId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

        public bool? Published { get; set; }

    }
}