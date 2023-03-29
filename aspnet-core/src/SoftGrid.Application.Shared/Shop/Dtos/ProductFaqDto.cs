using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductFaqDto : EntityDto<long>
    {
        public string Question { get; set; }

        public string Answer { get; set; }

        public bool Template { get; set; }

        public bool Publish { get; set; }

        public long ProductId { get; set; }

    }
}