using SoftGrid.Shop.Enums;

using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreMasterTagSettingDto : EntityDto<long>
    {
        public string CustomTagTitle { get; set; }

        public string CustomTagChatQuestion { get; set; }

        public bool DisplayPublic { get; set; }

        public int? DisplaySequence { get; set; }

        public AnswerType AnswerTypeId { get; set; }

        public long StoreTagSettingCategoryId { get; set; }

        public long? MasterTagCategoryId { get; set; }

    }
}