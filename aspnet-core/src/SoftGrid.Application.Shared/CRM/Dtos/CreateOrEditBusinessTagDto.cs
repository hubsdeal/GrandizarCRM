using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditBusinessTagDto : EntityDto<long?>
    {

        [StringLength(BusinessTagConsts.MaxCustomTagLength, MinimumLength = BusinessTagConsts.MinCustomTagLength)]
        public string CustomTag { get; set; }

        [StringLength(BusinessTagConsts.MaxTagValueLength, MinimumLength = BusinessTagConsts.MinTagValueLength)]
        public string TagValue { get; set; }

        public bool Verified { get; set; }

        public int? Sequence { get; set; }

        public long BusinessId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

    }
}