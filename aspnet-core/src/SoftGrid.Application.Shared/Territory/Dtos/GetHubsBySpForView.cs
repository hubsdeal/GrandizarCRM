using SoftGrid.Shop.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftGrid.Territory.Dtos
{
    public class GetHubsBySpForView
    {
        public int TotalCount { get; set; }
        public List<HubFromSpDto> Hubs { get; set; }

        public GetHubsBySpForView()
        {
            Hubs = new List<HubFromSpDto>();
        }
    }

    public class HubFromSpDto : HubDto
    {
        public string CountryName { get; set; }

        public string StateName { get; set; }

        public string CityName { get; set; }

        public string HubTypeName { get; set; }
        public string Picture { get; set; }

        public int NumberOfStores { get; set; }
        public int NumberOfContacts { get; set; }
        public int NumberOfBusinesses { get; set; }

        public Guid? BinaryObjectId { get; set; }
    }
}
