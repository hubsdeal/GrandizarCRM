using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetAllOrderSalesChannelsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string LinkNameFilter { get; set; }

        public string ApiLinkFilter { get; set; }

        public string UserIdFilter { get; set; }

        public string PasswordFilter { get; set; }

        public string NotesFilter { get; set; }

    }
}