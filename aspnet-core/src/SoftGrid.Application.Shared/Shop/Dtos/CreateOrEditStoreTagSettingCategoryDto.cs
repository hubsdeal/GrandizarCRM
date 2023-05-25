using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreTagSettingCategoryDto : EntityDto<long?>
    {

        [Required]
        [StringLength(StoreTagSettingCategoryConsts.MaxNameLength, MinimumLength = StoreTagSettingCategoryConsts.MinNameLength)]
        public string Name { get; set; }

        public Guid ImageId { get; set; }

        [StringLength(StoreTagSettingCategoryConsts.MaxDescriptionLength, MinimumLength = StoreTagSettingCategoryConsts.MinDescriptionLength)]
        public string Description { get; set; }

    }
}