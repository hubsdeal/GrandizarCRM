using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class ProductWholeSaleQuantityTypeDto : EntityDto<long>
    {
        public string Name { get; set; }

        public int? MinQty { get; set; }

        public int? MaxQty { get; set; }

    }
}