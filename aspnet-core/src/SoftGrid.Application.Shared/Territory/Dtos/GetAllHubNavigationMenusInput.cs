using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Territory.Dtos
{
    public class GetAllHubNavigationMenusInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CustomNameFilter { get; set; }

        public string NavigationLinkFilter { get; set; }

        public string HubNameFilter { get; set; }

        public string MasterNavigationMenuNameFilter { get; set; }

    }
}