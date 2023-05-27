using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllContactMasterTagSettingsForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxDisplaySequenceFilter { get; set; }
        public int? MinDisplaySequenceFilter { get; set; }

        public int? DisplayPublicFilter { get; set; }

        public string CustomTagTitleFilter { get; set; }

        public string CustomTagChatQuestionFilter { get; set; }

        public int? AnswerTypeIdFilter { get; set; }

        public string MasterTagNameFilter { get; set; }

        public string MasterTagCategoryNameFilter { get; set; }

    }
}