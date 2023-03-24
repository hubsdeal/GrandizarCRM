using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.TaskManagement.Dtos
{
    public class TaskTagDto : EntityDto<long>
    {
        public string CustomTag { get; set; }

        public string TagValue { get; set; }

        public bool Verfied { get; set; }

        public int? Sequence { get; set; }

        public long TaskEventId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

    }
}