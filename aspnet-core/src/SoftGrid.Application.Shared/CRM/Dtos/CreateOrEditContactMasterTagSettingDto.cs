using SoftGrid.Shop.Enums;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditContactMasterTagSettingDto : EntityDto<long?>
    {

        public int? DisplaySequence { get; set; }

        public bool DisplayPublic { get; set; }

        [StringLength(ContactMasterTagSettingConsts.MaxCustomTagTitleLength, MinimumLength = ContactMasterTagSettingConsts.MinCustomTagTitleLength)]
        public string CustomTagTitle { get; set; }

        [StringLength(ContactMasterTagSettingConsts.MaxCustomTagChatQuestionLength, MinimumLength = ContactMasterTagSettingConsts.MinCustomTagChatQuestionLength)]
        public string CustomTagChatQuestion { get; set; }

        public AnswerType AnswerTypeId { get; set; }

        public long ContactTypeId { get; set; }

        public long? MasterTagCategoryId { get; set; }

    }
}