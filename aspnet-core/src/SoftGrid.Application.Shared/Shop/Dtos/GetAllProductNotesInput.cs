using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductNotesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NotesFilter { get; set; }

        public string ProductNameFilter { get; set; }

    }
}