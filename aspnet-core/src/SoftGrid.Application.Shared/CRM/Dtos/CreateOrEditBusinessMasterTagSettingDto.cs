using SoftGrid.Shop.Enums;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditBusinessMasterTagSettingDto : EntityDto<long?>
    {

        public int? DisplaySequence { get; set; }

        public bool DisplayPublic { get; set; }

        [StringLength(BusinessMasterTagSettingConsts.MaxCustomTagTitleLength, MinimumLength = BusinessMasterTagSettingConsts.MinCustomTagTitleLength)]
        public string CustomTagTitle { get; set; }

        [StringLength(BusinessMasterTagSettingConsts.MaxCustomTagChatQuestionLength, MinimumLength = BusinessMasterTagSettingConsts.MinCustomTagChatQuestionLength)]
        public string CustomTagChatQuestion { get; set; }

        public AnswerType AnswerTypeId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long BusinessTypeId { get; set; }

    }
}