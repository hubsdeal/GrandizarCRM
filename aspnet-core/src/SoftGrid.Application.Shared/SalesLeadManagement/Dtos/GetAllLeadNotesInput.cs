using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetAllLeadNotesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NotesFilter { get; set; }

        public string LeadTitleFilter { get; set; }

    }
}