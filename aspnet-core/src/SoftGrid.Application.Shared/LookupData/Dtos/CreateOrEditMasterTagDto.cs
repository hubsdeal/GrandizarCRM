using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditMasterTagDto : EntityDto<long?>
    {

        [Required]
        [StringLength(MasterTagConsts.MaxNameLength, MinimumLength = MasterTagConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(MasterTagConsts.MaxDescriptionLength, MinimumLength = MasterTagConsts.MinDescriptionLength)]
        public string Description { get; set; }

        [StringLength(MasterTagConsts.MaxSynonymsLength, MinimumLength = MasterTagConsts.MinSynonymsLength)]
        public string Synonyms { get; set; }

        public Guid PictureId { get; set; }

        public int? DisplaySequence { get; set; }

        public long MasterTagCategoryId { get; set; }

    }
}