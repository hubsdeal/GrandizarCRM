using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Territory.Dtos
{
    public class GetAllHubContactsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxDisplayScoreFilter { get; set; }
        public int? MinDisplayScoreFilter { get; set; }

        public string HubNameFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

    }
}