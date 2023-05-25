using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreTagSettingCategoryDto : EntityDto<long>
    {
        public string Name { get; set; }

        public Guid ImageId { get; set; }

        public string Description { get; set; }

    }
}