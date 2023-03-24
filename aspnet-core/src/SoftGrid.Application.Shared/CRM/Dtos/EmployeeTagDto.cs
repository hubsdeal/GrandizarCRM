using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.CRM.Dtos
{
    public class EmployeeTagDto : EntityDto<long>
    {
        public string CustomTag { get; set; }

        public string TagValue { get; set; }

        public bool Verified { get; set; }

        public int? Sequence { get; set; }

        public long EmployeeId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

    }
}