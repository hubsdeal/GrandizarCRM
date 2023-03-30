using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductReturnInfoDto : EntityDto<long>
    {
        public string CustomerNote { get; set; }

        public string AdminNote { get; set; }

        public long ProductId { get; set; }

        public long? ReturnTypeId { get; set; }

        public long? ReturnStatusId { get; set; }

    }
}