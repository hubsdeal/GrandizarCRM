using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.JobManagement.Dtos
{
    public class GetAllJobMasterTagSettingsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxDisplaySequenceFilter { get; set; }
        public int? MinDisplaySequenceFilter { get; set; }

        public int? DisplayPublicFilter { get; set; }

        public int? AnswerTypeIdFilter { get; set; }

        public string CustomTagTitleFilter { get; set; }

        public string CustomTagChatQuestionFilter { get; set; }

        public string MasterTagNameFilter { get; set; }

        public string MasterTagCategoryNameFilter { get; set; }

    }
}