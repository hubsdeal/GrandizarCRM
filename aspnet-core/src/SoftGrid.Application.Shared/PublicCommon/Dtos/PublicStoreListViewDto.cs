using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftGrid.PublicCommon.Dtos
{
    public class PublicStoreListViewDto: RelevantStorePublicViewDto
    {
        public List<string> StoreTags { get; set; }
        public GetStoreHourCurrentStatusForPublicViewDto OpenHour { get; set; }

        public PublicStoreListViewDto()
        {
            StoreTags = new List<string>();
        }
    }

    public class PublicStoreListInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public long? HubId { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }
    }

    public class PublicHubsListInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public long? StoreIdFilter { get; set; }
        public long? CountryIdFilter { get; set; }
        public string StateName { get; set; }
        public string CountyName { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
    }

    public class PublicStoreListByHubInput
    {
        public string Filter { get; set; }
        public long HubId { get; set; }
        public int? DeliveryTypeId { get; set; }
    }

    public class GetAllHubWiseStoreInput : PagedResultRequestDto
    {
        public string Filter { get; set; }
        public long HubId { get; set; }
        public int? DeliveryTypeId { get; set; }

    }

    public class PublicProductListByHubInput: PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public long HubId { get; set; }
    }
}
