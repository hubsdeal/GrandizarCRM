using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductTaskMapDto : EntityDto<long>
    {

        public long? ProductId { get; set; }

        public long TaskEventId { get; set; }

        public long? ProductCategoryId { get; set; }

    }
}