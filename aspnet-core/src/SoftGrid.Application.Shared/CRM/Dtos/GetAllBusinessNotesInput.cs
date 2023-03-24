using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllBusinessNotesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NotesFilter { get; set; }

        public string BusinessNameFilter { get; set; }

    }
}