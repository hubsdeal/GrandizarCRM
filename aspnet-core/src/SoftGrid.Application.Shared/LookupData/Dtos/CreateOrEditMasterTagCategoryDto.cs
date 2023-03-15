using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditMasterTagCategoryDto : EntityDto<long?>
    {

        [Required]
        [StringLength(MasterTagCategoryConsts.MaxNameLength, MinimumLength = MasterTagCategoryConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(MasterTagCategoryConsts.MaxDescriptionLength, MinimumLength = MasterTagCategoryConsts.MinDescriptionLength)]
        public string Description { get; set; }

        public long? PictureMediaLibraryId { get; set; }

    }
}