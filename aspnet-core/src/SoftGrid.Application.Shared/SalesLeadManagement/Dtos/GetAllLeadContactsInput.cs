using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetAllLeadContactsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NotesFilter { get; set; }

        public int? MaxInfluenceScoreFilter { get; set; }
        public int? MinInfluenceScoreFilter { get; set; }

        public string LeadTitleFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

    }
}