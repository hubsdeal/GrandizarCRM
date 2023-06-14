using Abp.Application.Services.Dto;

namespace SoftGrid.Territory.Dtos
{
    public class HubNavigationMenuDto : EntityDto<long>
    {
        public string CustomName { get; set; }

        public string NavigationLink { get; set; }

        public long HubId { get; set; }

        public long? MasterNavigationMenuId { get; set; }

        public bool? HasParentMenu { get; set; }
        public long? ParentMenuId { get; set; }
        public int? DisplaySequence { get; set; }
        public string HubName { get; set; }
        public int? TenantId { get; set; }
        public string MenuName { get; set; }
    }
}