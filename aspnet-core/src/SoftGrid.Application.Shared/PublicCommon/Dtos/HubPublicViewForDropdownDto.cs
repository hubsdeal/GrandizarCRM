using System;
using System.Collections.Generic;
using System.Text;

namespace SoftGrid.PublicCommon.Dtos
{
    public class HubPublicViewForDropdownDto
    {
        public long Id { get; set; }

        public string DisplayName { get; set; }

        public string Picture { get; set; }
        public Guid PictureId { get; set; }

        public string Url { get; set; }
    }

    public class GetAllHubsForDropdownViewBySp
    {
        public List<HubPublicViewForDropdownDto> Hubs { get; set; }

        public GetAllHubsForDropdownViewBySp()
        {
            Hubs = new List<HubPublicViewForDropdownDto>();
        }
    }
}
