using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreProductMapDto : EntityDto<long>
    {
        public bool Published { get; set; }

        public int? DisplaySequence { get; set; }

        public long StoreId { get; set; }

        public long ProductId { get; set; }

    }
}