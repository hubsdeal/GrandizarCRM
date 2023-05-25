using SoftGrid.Shop.Enums;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductMasterTagSettingDto : EntityDto<long?>
    {

        public int? DisplaySequence { get; set; }

        [Required]
        [StringLength(ProductMasterTagSettingConsts.MaxCustomTagTitleLength, MinimumLength = ProductMasterTagSettingConsts.MinCustomTagTitleLength)]
        public string CustomTagTitle { get; set; }

        [StringLength(ProductMasterTagSettingConsts.MaxCustomTagChatQuestionLength, MinimumLength = ProductMasterTagSettingConsts.MinCustomTagChatQuestionLength)]
        public string CustomTagChatQuestion { get; set; }

        public bool DisplayPublic { get; set; }

        public AnswerType AnswerTypeId { get; set; }

        public long ProductCategoryId { get; set; }

        public long? MasterTagCategoryId { get; set; }

    }
}