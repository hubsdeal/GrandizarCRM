using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class CreateOrEditLeadTagDto : EntityDto<long?>
    {

        [StringLength(LeadTagConsts.MaxCustomTagLength, MinimumLength = LeadTagConsts.MinCustomTagLength)]
        public string CustomTag { get; set; }

        [StringLength(LeadTagConsts.MaxTagValueLength, MinimumLength = LeadTagConsts.MinTagValueLength)]
        public string TagValue { get; set; }

        public int? DisplaySequence { get; set; }

        public long LeadId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

    }
}