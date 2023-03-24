using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.JobManagement.Dtos
{
    public class CreateOrEditJobTagDto : EntityDto<long?>
    {

        [StringLength(JobTagConsts.MaxCustomTagLength, MinimumLength = JobTagConsts.MinCustomTagLength)]
        public string CustomTag { get; set; }

        [StringLength(JobTagConsts.MaxTagValueLength, MinimumLength = JobTagConsts.MinTagValueLength)]
        public string TagValue { get; set; }

        public bool Verified { get; set; }

        public int? Sequence { get; set; }

        public long JobId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

    }
}