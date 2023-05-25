using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreMasterTagSettingsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CustomTagTitleFilter { get; set; }

        public string CustomTagChatQuestionFilter { get; set; }

        public int? DisplayPublicFilter { get; set; }

        public int? MaxDisplaySequenceFilter { get; set; }
        public int? MinDisplaySequenceFilter { get; set; }

        public int? AnswerTypeIdFilter { get; set; }

        public string StoreTagSettingCategoryNameFilter { get; set; }

        public string MasterTagCategoryNameFilter { get; set; }

    }
}