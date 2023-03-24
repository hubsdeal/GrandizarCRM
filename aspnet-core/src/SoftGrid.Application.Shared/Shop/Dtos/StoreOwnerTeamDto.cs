using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreOwnerTeamDto : EntityDto<long>
    {
        public bool Active { get; set; }

        public bool Primary { get; set; }

        public bool OrderEmailNotification { get; set; }

        public bool OrderSmsNotification { get; set; }

        public long StoreId { get; set; }

        public long UserId { get; set; }

    }
}