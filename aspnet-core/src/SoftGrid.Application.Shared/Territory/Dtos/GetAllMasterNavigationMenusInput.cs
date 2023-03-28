using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Territory.Dtos
{
    public class GetAllMasterNavigationMenusInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? HasParentMenuFilter { get; set; }

        public long? MaxParentMenuIdFilter { get; set; }
        public long? MinParentMenuIdFilter { get; set; }

        public Guid? IconLinkFilter { get; set; }

        public Guid? MediaLinkFilter { get; set; }

    }
}