using SoftGrid.Shop.Enums;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreMasterTagSettingDto : EntityDto<long?>
    {

        [StringLength(StoreMasterTagSettingConsts.MaxCustomTagTitleLength, MinimumLength = StoreMasterTagSettingConsts.MinCustomTagTitleLength)]
        public string CustomTagTitle { get; set; }

        [StringLength(StoreMasterTagSettingConsts.MaxCustomTagChatQuestionLength, MinimumLength = StoreMasterTagSettingConsts.MinCustomTagChatQuestionLength)]
        public string CustomTagChatQuestion { get; set; }

        public bool DisplayPublic { get; set; }

        public int? DisplaySequence { get; set; }

        public AnswerType AnswerTypeId { get; set; }

        public long StoreTagSettingCategoryId { get; set; }

        public long? MasterTagCategoryId { get; set; }

    }
}