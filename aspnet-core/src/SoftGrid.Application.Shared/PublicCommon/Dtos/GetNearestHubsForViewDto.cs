using System;
using System.Collections.Generic;
using System.Text;

namespace SoftGrid.PublicCommon.Dtos
{
    public class GetNearestHubsForViewDto
    {
        public GetNearestHubsForViewDto()
        {
            NearestHubs = new List<NearestHubPublicViewDto>();
        }
       public List<NearestHubPublicViewDto> NearestHubs { get; set; }
    }

    public class NearestHubPublicViewDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal? distance { get; set; }
        public Guid? PictureId { get; set; }
        public string Picture { get; set; }
    }
}
