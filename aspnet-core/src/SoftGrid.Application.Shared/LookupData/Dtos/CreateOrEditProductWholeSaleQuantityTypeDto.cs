using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditProductWholeSaleQuantityTypeDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ProductWholeSaleQuantityTypeConsts.MaxNameLength, MinimumLength = ProductWholeSaleQuantityTypeConsts.MinNameLength)]
        public string Name { get; set; }

        public int? MinQty { get; set; }

        public int? MaxQty { get; set; }

    }
}