using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Territory.Dtos
{
    public class HubNavigationMenuDto : EntityDto<long>
    {
        public string CustomName { get; set; }

        public string NavigationLink { get; set; }

        public long HubId { get; set; }

        public long? MasterNavigationMenuId { get; set; }

    }
}