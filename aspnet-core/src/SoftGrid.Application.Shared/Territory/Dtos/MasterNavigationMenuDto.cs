using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Territory.Dtos
{
    public class MasterNavigationMenuDto : EntityDto<long>
    {
        public string Name { get; set; }

        public bool HasParentMenu { get; set; }

        public long? ParentMenuId { get; set; }

        public Guid IconLink { get; set; }

        public Guid MediaLink { get; set; }

    }
}