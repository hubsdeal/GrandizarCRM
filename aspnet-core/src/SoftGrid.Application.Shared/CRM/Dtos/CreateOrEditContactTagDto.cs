using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditContactTagDto : EntityDto<long?>
    {

        [StringLength(ContactTagConsts.MaxCustomTagLength, MinimumLength = ContactTagConsts.MinCustomTagLength)]
        public string CustomTag { get; set; }

        [StringLength(ContactTagConsts.MaxTagValueLength, MinimumLength = ContactTagConsts.MinTagValueLength)]
        public string TagValue { get; set; }

        public bool Verified { get; set; }

        public int? Sequence { get; set; }

        public long ContactId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

    }
}