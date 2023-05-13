using SoftGrid.Shop.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftGrid.PublicCommon.Dtos
{
    public class RelevantStorePublicViewDto
    {
        
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Logo { get; set; }

        public Guid? StoreLogoLink { get; set; }
        public int? DisplaySequence { get; set; }
    }

    public class RelevantStorePublicWidgetMapDto
    {
        public StoreDynamicWidgetMapDto StoreDynamicWidgetMap { get; set; }
        public List<RelevantStorePublicViewDto> Stores { get; set; }

        public RelevantStorePublicWidgetMapDto()
        {
            Stores = new List<RelevantStorePublicViewDto>();
        }
    }
}
