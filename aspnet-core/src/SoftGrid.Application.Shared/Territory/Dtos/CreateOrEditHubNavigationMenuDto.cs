using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class CreateOrEditHubNavigationMenuDto : EntityDto<long?>
    {

        [StringLength(HubNavigationMenuConsts.MaxCustomNameLength, MinimumLength = HubNavigationMenuConsts.MinCustomNameLength)]
        public string CustomName { get; set; }

        public string NavigationLink { get; set; }

        public long HubId { get; set; }

        public long? MasterNavigationMenuId { get; set; }

        public bool? HasParentMenu { get; set; }
        public long? ParentMenuId { get; set; }
        public int? DisplaySequence { get; set; }

    }
}