using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.CRM.Dtos
{
    public class GetAllBusinessNotesForExcelInput
    {
        public string Filter { get; set; }

        public string NotesFilter { get; set; }

        public string BusinessNameFilter { get; set; }

    }
}