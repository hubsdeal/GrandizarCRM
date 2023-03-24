using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreTagDto : EntityDto<long?>
    {

        [StringLength(StoreTagConsts.MaxCustomTagLength, MinimumLength = StoreTagConsts.MinCustomTagLength)]
        public string CustomTag { get; set; }

        [StringLength(StoreTagConsts.MaxTagValueLength, MinimumLength = StoreTagConsts.MinTagValueLength)]
        public string TagValue { get; set; }

        public bool Verified { get; set; }

        public int? Sequence { get; set; }

        public long StoreId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

    }
}