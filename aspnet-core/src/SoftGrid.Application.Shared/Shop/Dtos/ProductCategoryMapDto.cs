using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductCategoryMapDto : EntityDto<long>
    {

        public long ProductId { get; set; }

        public long ProductCategoryId { get; set; }

    }
}