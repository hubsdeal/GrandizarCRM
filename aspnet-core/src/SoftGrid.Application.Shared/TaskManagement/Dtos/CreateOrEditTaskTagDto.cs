using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.TaskManagement.Dtos
{
    public class CreateOrEditTaskTagDto : EntityDto<long?>
    {

        [StringLength(TaskTagConsts.MaxCustomTagLength, MinimumLength = TaskTagConsts.MinCustomTagLength)]
        public string CustomTag { get; set; }

        [StringLength(TaskTagConsts.MaxTagValueLength, MinimumLength = TaskTagConsts.MinTagValueLength)]
        public string TagValue { get; set; }

        public bool Verfied { get; set; }

        public int? Sequence { get; set; }

        public long TaskEventId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

    }
}