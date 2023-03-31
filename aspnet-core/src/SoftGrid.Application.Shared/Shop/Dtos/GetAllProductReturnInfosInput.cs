using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductReturnInfosInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CustomerNoteFilter { get; set; }

        public string AdminNoteFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string ReturnTypeNameFilter { get; set; }

        public string ReturnStatusNameFilter { get; set; }

    }
}