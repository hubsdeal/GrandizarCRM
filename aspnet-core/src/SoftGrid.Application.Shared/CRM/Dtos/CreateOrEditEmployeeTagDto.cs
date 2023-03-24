using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditEmployeeTagDto : EntityDto<long?>
    {

        [StringLength(EmployeeTagConsts.MaxCustomTagLength, MinimumLength = EmployeeTagConsts.MinCustomTagLength)]
        public string CustomTag { get; set; }

        [StringLength(EmployeeTagConsts.MaxTagValueLength, MinimumLength = EmployeeTagConsts.MinTagValueLength)]
        public string TagValue { get; set; }

        public bool Verified { get; set; }

        public int? Sequence { get; set; }

        public long EmployeeId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

    }
}