using SoftGrid.Shop.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftGrid.PublicCommon.Dtos
{
    public class TopHubPublicViewDto
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public Guid? PictureLink { get; set; }
        public int? DisplaySequence { get; set; }
    }

    public class TopHubsPublicWidgetMapDto
    {
        public StoreDynamicWidgetMapDto StoreDynamicWidgetMap { get; set; }
        public List<TopHubPublicViewDto> Hubs { get; set; }

        public TopHubsPublicWidgetMapDto()
        {
            Hubs = new List<TopHubPublicViewDto>();
        }
    }

    public class GetHubListForPublicDirectoryBySp
    {
        public int TotalCount { get; set; }
        public List<TopHubPublicViewDto> Hubs { get; set; }
    }
}
