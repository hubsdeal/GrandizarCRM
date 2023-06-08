using Abp.Application.Services.Dto;

using SoftGrid.Shop.Dtos;

using System.Collections.Generic;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class HubWidgetStoreMapDto : EntityDto<long>
    {
        public int? DisplaySequence { get; set; }

        public long HubWidgetMapId { get; set; }

        public long StoreId { get; set; }

    }


    public class HubWidgetStoreMapDtoForView
    {
        public long Id { get; set; }
        public int? DisplaySequence { get; set; }
        public long? HubWidgetMapId { get; set; }
        public long? StoreId { get; set; }

        public List<StoreDto> StoreDtos { get; set; }

    }



}